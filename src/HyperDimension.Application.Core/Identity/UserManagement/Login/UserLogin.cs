using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Attributes;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HyperDimension.Application.Core.Identity.UserManagement.Login;

[AllowAnonymous]
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
    private readonly IPasswordHashService _passwordHashService;

    public UserLoginHandler(
        IHyperDimensionDbContext dbContext,
        IPasswordHashService passwordHashService)
    {
        _dbContext = dbContext;
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
            return new ErrorMessageResult("User not found").ToBadRequest();
        }

        // Locked out
        if (user.LockoutEndAt is not null && user.LockoutEndAt > DateTimeOffset.UtcNow)
        {
            return new ErrorMessageResult($"User is locked out, please retry after {user.LockoutEndAt}").ToBadRequest();
        }

        // Verify password
        var passwordVerifyResult = _passwordHashService.VerifyPassword(body.Password, user.SecurityStamp, user.PasswordHash);
        if (passwordVerifyResult is false)
        {
            // Add failed login attempt
            user.FailedAccessAttempts++;

            // Check if user should be locked out
            if (user.FailedAccessAttempts >= 1 && user.FailedAccessAttempts % 5 == 0)
            {
                var lockedOutFactor = user.FailedAccessAttempts / 5;
                var lockedOutDuration = TimeSpan.FromMinutes(5 * lockedOutFactor);
                user.LockoutEndAt = DateTimeOffset.UtcNow.Add(lockedOutDuration);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new ErrorMessageResult("Password is incorrect").ToBadRequest();
        }

        // Reset failed login attempt
        user.FailedAccessAttempts = 0;
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Check if 2FA is enabled
        if (user.TwoFactorEnabled)
        {
            return new SignInResult(user.CreateTwoFactorClaimsPrincipal());
        }

        return new SignInResult(user.CreateIdentityClaimsPrincipal());
    }
}
