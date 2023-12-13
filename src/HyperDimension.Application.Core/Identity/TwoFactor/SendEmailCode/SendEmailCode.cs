using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Constants;
using HyperDimension.Domain.Email;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.TwoFactor.SendEmailCode;

[RequireAuthentication(IdentityConstants.IdentitySchema, IdentityConstants.TwoFactorSchema)]
public class SendEmailCode : IRequest<IActionResult>;

public class SendEmailCodeHandler : IRequestHandler<SendEmailCode, IActionResult>
{
    private readonly IHyperDimensionRequestContext _requestContext;
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IStringLocalizer<SendEmailCodeHandler> _localizer;
    private readonly ISecurityTokenService _securityTokenService;
    private readonly IEmailService _emailService;

    public SendEmailCodeHandler(
        IHyperDimensionRequestContext requestContext,
        IHyperDimensionDbContext dbContext,
        IStringLocalizer<SendEmailCodeHandler> localizer,
        ISecurityTokenService securityTokenService,
        IEmailService emailService)
    {
        _requestContext = requestContext;
        _dbContext = dbContext;
        _localizer = localizer;
        _securityTokenService = securityTokenService;
        _emailService = emailService;
    }

    public async Task<IActionResult> Handle(SendEmailCode request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstAsync(u => u.EntityId == _requestContext.UserId, cancellationToken);

        var email = user.Email;

        var now = DateTimeOffset.UtcNow;

        var tokenResult = await _securityTokenService.CreateTokenAsync(
            user.EntityId, TokenUsage.EmailTwoFactorAuthentication, now, cancellationToken);

        if (tokenResult.IsFailure)
        {
            return new ErrorMessageResult(_localizer["You already have a valid code been sent."]).ToBadRequest();
        }

        var token = tokenResult.Unwrap();

        var emailSendResult = await _emailService.SendEmailAsync(email, new TwoFactorAuthenticationCode
        {
            User = user,
            Code = token.Value
        });

        if (emailSendResult.IsFailure)
        {
            return new ErrorMessageResult(_localizer["Failed to send two factor authentication code email."]).ToBadRequest();
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new NoContentResult();
    }
}
