using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.User.RevokeToken;

public class RevokeApiToken : IRequest<IActionResult>
{
    [FromBody]
    public RevokeApiTokenBody Body { get; set; } = new();
}

public class RevokeApiTokenBody
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}

public class RevokeApiTokenHandler : IRequestHandler<RevokeApiToken, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IStringLocalizer<RevokeApiTokenHandler> _localizer;
    private readonly IHyperDimensionRequestContext _requestContext;

    public RevokeApiTokenHandler(
        IHyperDimensionDbContext dbContext,
        IStringLocalizer<RevokeApiTokenHandler> localizer,
        IHyperDimensionRequestContext requestContext)
    {
        _dbContext = dbContext;
        _localizer = localizer;
        _requestContext = requestContext;
    }

    public async Task<IActionResult> Handle(RevokeApiToken request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x => x.ApiTokens)
            .FirstAsync(x => x.EntityId == _requestContext.UserId, cancellationToken);

        var token = user.ApiTokens
            .FirstOrDefault(x => x.Name == request.Body.Name);

        if (token is null)
        {
            return new ErrorMessageResult(_localizer["Api token with name {0} does not exist."].Format(request.Body.Name)).ToBadRequest();
        }

        user.ApiTokens.Remove(token);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new NoContentResult();
    }
}
