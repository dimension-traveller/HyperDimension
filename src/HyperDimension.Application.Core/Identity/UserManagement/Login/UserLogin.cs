using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Extensions;
using HyperDimension.Domain.Email;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.UserManagement.Login;

public class UserLogin : IRequest<IActionResult>
{
    [FromBody]
    public UserLoginBody Body { get; set; } = new();
}

public class UserLoginBody
{
    [JsonPropertyName("login_name")]
    public string LoginName { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class UserLoginHandler : IRequestHandler<UserLogin, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly IStringLocalizer<UserLoginHandler> _localizer;
    private readonly ISecurityTokenService _securityTokenService;
    private readonly IPasswordHashService _passwordHashService;

    public UserLoginHandler(
        IHyperDimensionDbContext dbContext,
        IEmailService emailService,
        IStringLocalizer<UserLoginHandler> localizer,
        ISecurityTokenService securityTokenService,
        IPasswordHashService passwordHashService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        _localizer = localizer;
        _securityTokenService = securityTokenService;
        _passwordHashService = passwordHashService;
    }

    public async Task<IActionResult> Handle(UserLogin request, CancellationToken cancellationToken)
    {
        var body = request.Body;

        // Get user
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Username == body.LoginName || x.Email == body.LoginName, cancellationToken);

        // Not found
        if (user is null)
        {
            return new ErrorMessageResult(_localizer["User not found."]).ToBadRequest();
        }

        // Locked out
        if (user.LockoutEndAt is not null && user.LockoutEndAt > DateTimeOffset.UtcNow)
        {
            return new ErrorMessageResult(_localizer["User is locked out, please try after {0}."].Format(user.LockoutEndAt)).ToBadRequest();
        }

        // Verify password
        var passwordVerifyResult = _passwordHashService.VerifyPassword(body.Password, user.SecurityStamp, user.PasswordHash);
        if (passwordVerifyResult is false)
        {
            // Add failed login attempt
            user.FailedAccessAttempts++;

            var failedMessage = _localizer["Password is incorrect."].ToString();

            // Check if user should be locked out
            if (user.FailedAccessAttempts >= 1 && user.FailedAccessAttempts % 5 == 0)
            {
                var lockedOutFactor = user.FailedAccessAttempts / 5;
                var lockedOutDuration = TimeSpan.FromMinutes(5 * lockedOutFactor);
                user.LockoutEndAt = DateTimeOffset.UtcNow.Add(lockedOutDuration);

                failedMessage += _localizer["User is locked out, please try after {0}."].Format(user.LockoutEndAt);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ErrorMessageResult(failedMessage).ToBadRequest();
        }

        // Check if email is confirmed
        if (user.EmailConfirmed is false)
        {
            var now = DateTimeOffset.UtcNow;

            var tokenResult = await _securityTokenService
                .CreateTokenAsync(user.EntityId, TokenUsage.AccountVerification, now, cancellationToken);

            if (tokenResult.IsFailure)
            {
                return new ErrorMessageResult(_localizer["Email is not confirmed."]).ToBadRequest();
            }

            var token = tokenResult.Unwrap();

            var emailSendResult = await _emailService.SendEmailAsync(user.Email, new AccountVerification
            {
                User = user,
                Token = token.Value
            });

            if (emailSendResult.IsFailure)
            {
                return new ErrorMessageResult(_localizer["Failed to send account verification email."]).ToBadRequest();
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ErrorMessageResult(_localizer["Email is not confirmed, a new verification email has been sent to your email address."]).ToBadRequest();
        }

        // Reset failed login attempt
        user.FailedAccessAttempts = 0;
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Check if 2FA is enabled
        if (user.TwoFactorEmailEnabled || user.TwoFactorTotpEnabled)
        {
            return new SignInResult(user.CreateTwoFactorClaimsPrincipal());
        }

        return new SignInResult(user.CreateIdentityClaimsPrincipal());
    }
}
