using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Constants;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.TwoFactor.Login;

[RequireAuthentication(IdentityConstants.TwoFactorSchema)]
public class TwoFactorLogin : IRequest<IActionResult>
{
    [FromBody]
    public TwoFactorLoginBody Body { get; set; } = new();
}

public class TwoFactorLoginBody
{
    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;
}

public class TwoFactorLoginHandler : IRequestHandler<TwoFactorLogin, IActionResult>
{
    private readonly ITotpService _totpService;
    private readonly IStringLocalizer<TwoFactorLoginHandler> _localizer;
    private readonly IHyperDimensionRequestContext _requestContext;
    private readonly IHyperDimensionDbContext _dbContext;

    public TwoFactorLoginHandler(
        ITotpService totpService,
        IStringLocalizer<TwoFactorLoginHandler> localizer,
        IHyperDimensionRequestContext requestContext,
        IHyperDimensionDbContext dbContext)
    {
        _totpService = totpService;
        _localizer = localizer;
        _requestContext = requestContext;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(TwoFactorLogin request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Include(x => x.Totp)
            .ThenInclude(x => x.RecoveryCodes)
            .FirstAsync(u => u.EntityId == _requestContext.UserId, cancellationToken);

        var code = request.Body.Code;
        var time = request.Body.Time;

        if (user.TwoFactorTotpEnabled)
        {
            // Verify TOTP
            var totpKey = user.Totp.Key;
            var totpResult = _totpService.VerifyTotpCode(totpKey, code, time);

            if (totpResult)
            {
                return new SignInResult(user.CreateIdentityClaimsPrincipal());
            }

            // Verify Recovery Token
            var recoveryCode = user.Totp.RecoveryCodes
                .Where(x => x.IsUsed is false)
                .FirstOrDefault(x => x.Code == code);
            if (recoveryCode is not null)
            {
                recoveryCode.UsedAt = time;
                await _dbContext.SaveChangesAsync(cancellationToken);

                return new SignInResult(user.CreateIdentityClaimsPrincipal());
            }
        }

        // If TOTP is disabled, or verification failed, try to use email token
        var emailToken = await _dbContext.Tokens
            .Where(x => x.Usage == TokenUsage.EmailTwoFactorAuthentication)
            .Where(x => x.ExpiredAt > time)
            .FirstOrDefaultAsync(x => x.BindTo == user.EntityId, cancellationToken);

        if (emailToken is null || emailToken.Value != code)
        {
            return new ErrorMessageResult(_localizer["Invalid two factor authentication code."]).ToBadRequest();
        }

        _dbContext.Tokens.Remove(emailToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new SignInResult(user.CreateIdentityClaimsPrincipal());
    }
}

