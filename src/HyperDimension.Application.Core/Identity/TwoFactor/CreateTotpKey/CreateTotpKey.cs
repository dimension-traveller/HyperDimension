using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.TwoFactor.CreateTotpKey;

[RequireAuthentication]
public class CreateTotpKey : IRequest<IActionResult>;

public class CreateTotpKeyHandler : IRequestHandler<CreateTotpKey, IActionResult>
{
    private readonly ITotpService _totpService;
    private readonly IHyperDimensionRequestContext _requestContext;
    private readonly IStringLocalizer<CreateTotpKeyHandler> _localizer;
    private readonly IHyperDimensionDbContext _dbContext;

    public CreateTotpKeyHandler(
        ITotpService totpService,
        IHyperDimensionRequestContext requestContext,
        IStringLocalizer<CreateTotpKeyHandler> localizer,
        IHyperDimensionDbContext dbContext)
    {
        _totpService = totpService;
        _requestContext = requestContext;
        _localizer = localizer;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(CreateTotpKey request, CancellationToken cancellationToken)
    {
        // Check if user has already enabled 2FA
        var user = await _dbContext.Users
            .Include(x => x.Totp)
            .ThenInclude(x => x.RecoveryCodes)
            .FirstAsync(x => x.EntityId == _requestContext.UserId, cancellationToken);

        if (user.TwoFactorTotpEnabled)
        {
            return new ErrorMessageResult(_localizer["TOTP two factor authentication is already enabled."]).ToBadRequest();
        }

        // Create TOTP key and recovery codes
        var key = _totpService.GenerateKey().ToArray();
        var recoveryCodes = _totpService.GenerateRecoveryCodes();

        var now = DateTimeOffset.UtcNow;
        user.Totp = new Totp
        {
            Key = key,
            RegistrationTime = now,
            RecoveryCodes = recoveryCodes
                .Select(x => new TotpRecoveryCode
                {
                    Code = x,
                    CreatedAt = now
                }).ToList()
        };
        user.TwoFactorTotpEnabled = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        var totpUri = _totpService.GetTotpUri(key, user.Email);
        return new OkObjectResult(new CreateTotpKeyDto
        {
            Uri = totpUri
        });
    }
}
