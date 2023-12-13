using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.TwoFactor.EnableTwoFactor;

[RequireAuthentication]
public class EnableTwoFactor : IRequest<IActionResult>
{
    [FromBody]
    public EnableTwoFactorBody Body { get; set; } = new();
}

public class EnableTwoFactorBody
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "totp";

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;
}

public class EnableTwoFactorHandler : IRequestHandler<EnableTwoFactor, IActionResult>
{
    private readonly ITotpService _totpService;
    private readonly IHyperDimensionRequestContext _requestContext;
    private readonly IStringLocalizer<EnableTwoFactorHandler> _localizer;
    private readonly IHyperDimensionDbContext _dbContext;

    public EnableTwoFactorHandler(
        ITotpService totpService,
        IHyperDimensionRequestContext requestContext,
        IStringLocalizer<EnableTwoFactorHandler> localizer,
        IHyperDimensionDbContext dbContext)
    {
        _totpService = totpService;
        _requestContext = requestContext;
        _localizer = localizer;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(EnableTwoFactor request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x => x.Totp)
            .FirstAsync(x => x.EntityId == _requestContext.UserId, cancellationToken);

        switch (request.Body.Type)
        {
            case "totp" when user.TwoFactorTotpEnabled:
                return new ErrorMessageResult(_localizer["Two factor authentication with TOTP is already enabled."]).ToBadRequest();
            case "totp":
            {
                var totpVerified = _totpService.VerifyTotpCode(user.Totp.Key, request.Body.Code, request.Body.Time);

                if (totpVerified is false)
                {
                    return new ErrorMessageResult(_localizer["Failed to verify two factor authentication code."]).ToBadRequest();
                }

                user.TwoFactorTotpEnabled = true;
                break;
            }
            case "email" when user.TwoFactorEmailEnabled:
                return new ErrorMessageResult(_localizer["Two factor authentication with email is already enabled."]).ToBadRequest();
            case "email":
            {
                var emailToken = await _dbContext.Tokens
                    .Where(x => x.BindTo == user.EntityId)
                    .Where(x => x.Usage == TokenUsage.EmailTwoFactorAuthentication)
                    .Where(x => x.ExpiredAt >= request.Body.Time)
                    .FirstOrDefaultAsync(cancellationToken);

                var emailVerified = emailToken?.Value == request.Body.Code;

                if (emailVerified is false)
                {
                    return new ErrorMessageResult(_localizer["Failed to verify two factor authentication code."]).ToBadRequest();
                }

                user.TwoFactorEmailEnabled = true;
                break;
            }
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new NoContentResult();
    }
}
