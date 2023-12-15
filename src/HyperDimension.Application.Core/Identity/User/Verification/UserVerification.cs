using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Models;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.User.Verification;

public class UserVerification : IRequest<IActionResult>
{
    [FromBody]
    public UserVerificationBody Body { get; set; } = new();
}

public class UserVerificationBody
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;
}

public class UserVerificationHandler : IRequestHandler<UserVerification, IActionResult>
{
    private readonly IStringLocalizer<UserVerificationHandler> _localizer;
    private readonly IHyperDimensionDbContext _dbContext;

    public UserVerificationHandler(
        IStringLocalizer<UserVerificationHandler> localizer,
        IHyperDimensionDbContext dbContext)
    {
        _localizer = localizer;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(UserVerification request, CancellationToken cancellationToken)
    {
        var token = await _dbContext.Tokens
            .Where(x => x.Usage == TokenUsage.AccountVerification)
            .Where(x => x.ExpiredAt >= request.Body.Time)
            .Where(x => x.Value == request.Body.Code)
            .FirstOrDefaultAsync(cancellationToken);

        if (token is null)
        {
            return new ErrorMessageResult(_localizer["Invalid account verification code."]).ToBadRequest();
        }

        var user = await _dbContext.Users
            .Where(x => x.EntityId == token.BindTo)
            .FirstOrDefaultAsync(cancellationToken)
            .ExpectNotNull();

        user.EmailConfirmed = true;

        _dbContext.Tokens.Remove(token);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new NoContentResult();
    }
}
