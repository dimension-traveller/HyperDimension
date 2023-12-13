using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Domain.Email;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.UserManagement.PasswordResetRequest;

public class PasswordResetRequest : IRequest<IActionResult>
{
    [FromBody]
    public PasswordResetRequestBody Body { get; set; } = new();
}

public class PasswordResetRequestBody
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}

public class PasswordResetRequestHandler : IRequestHandler<PasswordResetRequest, IActionResult>
{
    private readonly ISecurityTokenService _securityTokenService;
    private readonly IStringLocalizer<PasswordResetRequestHandler> _localizer;
    private readonly IEmailService _emailService;
    private readonly IHyperDimensionDbContext _dbContext;

    public PasswordResetRequestHandler(
        ISecurityTokenService securityTokenService,
        IStringLocalizer<PasswordResetRequestHandler> localizer,
        IEmailService emailService,
        IHyperDimensionDbContext dbContext)
    {
        _securityTokenService = securityTokenService;
        _localizer = localizer;
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(PasswordResetRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Email == request.Body.Email, cancellationToken);

        // Prevent user enumeration
        if (user is null)
        {
            return new NoContentResult();
        }

        var now = DateTimeOffset.UtcNow;

        var token = await _securityTokenService.CreateTokenAsync(
            user.EntityId, TokenUsage.PasswordReset, now, cancellationToken);

        if (token.IsFailure)
        {
            return new ErrorMessageResult(_localizer["You already have a token for password reset."]).ToBadRequest();
        }

        var emailSendResult = await _emailService.SendEmailAsync(user.Email, new PasswordResetCode
        {
            User = user,
            Code = token.Unwrap().Value
        });

        if (emailSendResult.IsFailure)
        {
            return new ErrorMessageResult(_localizer["Failed to send password reset token email."]).ToBadRequest();
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return new NoContentResult();
    }
}
