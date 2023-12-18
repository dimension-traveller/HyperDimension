using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Extensions;
using HyperDimension.Common.Utilities;
using HyperDimension.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.User.GenerateToken;

[RequireAuthentication]
[Permission("hd.user.api_token")]
public class GenerateApiToken : IRequest<IActionResult>
{
    [FromBody]
    public GenerateApiTokenBody Body { get; set; } = new();
}

public class GenerateApiTokenBody
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("valid_for")]
    public int ValidFor { get; set; }
}

public class GenerateApiTokenHandler : IRequestHandler<GenerateApiToken, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IStringLocalizer<GenerateApiTokenHandler> _localizer;
    private readonly IHyperDimensionRequestContext _requestContext;

    public GenerateApiTokenHandler(
        IHyperDimensionDbContext dbContext,
        IStringLocalizer<GenerateApiTokenHandler> localizer,
        IHyperDimensionRequestContext requestContext)
    {
        _dbContext = dbContext;
        _localizer = localizer;
        _requestContext = requestContext;
    }

    public async Task<IActionResult> Handle(GenerateApiToken request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x => x.ApiTokens)
            .FirstAsync(x => x.EntityId == _requestContext.UserId, cancellationToken);

        var tokenValue = RandomUtility.GenerateToken(32, s =>
            user.ApiTokens.Exists(x => x.Token == s));

        if (user.ApiTokens.Exists(x => x.Name == request.Body.Name))
        {
            return new ErrorMessageResult(_localizer["Api token with name {0} already exists."].Format(request.Body.Name)).ToBadRequest();
        }

        var now = DateTimeOffset.UtcNow;
        var token = new ApiToken
        {
            Name = request.Body.Name,
            Token = tokenValue,
            CreatedAt = now,
            ExpiredAt = now.AddDays(request.Body.ValidFor),
            User = user
        };

        user.ApiTokens.Add(token);

        return new NoContentResult();
    }
}
