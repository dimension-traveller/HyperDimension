using System.Text.Json.Serialization;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Database;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Domain.Email;
using HyperDimension.Domain.Entities.Identity;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.UserManagement.Registration;

public class UserRegistration : IRequest<IActionResult>
{
    [FromBody]
    public UserRegistrationBody Body { get; set; } = new();
}

public class UserRegistrationBody
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("display_name")]
    public string DisplayName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;
}

public class UserRegistrationHandler : IRequestHandler<UserRegistration, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IEmailService _emailService;
    private readonly ISecurityTokenService _securityTokenService;
    private readonly IStringLocalizer<UserRegistrationHandler> _localizer;

    public UserRegistrationHandler(
        IHyperDimensionDbContext dbContext,
        IPasswordHashService passwordHashService,
        IEmailService emailService,
        ISecurityTokenService securityTokenService,
        IStringLocalizer<UserRegistrationHandler> localizer)
    {
        _dbContext = dbContext;
        _passwordHashService = passwordHashService;
        _emailService = emailService;
        _securityTokenService = securityTokenService;
        _localizer = localizer;
    }

    public async Task<IActionResult> Handle(UserRegistration request, CancellationToken cancellationToken)
    {
        var body = request.Body;

        // Check not exist
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(
                x => x.Email == body.Email || x.Username == body.Username,
                cancellationToken);
        if (existingUser is not null)
        {
            var message = string.Empty;
            if (existingUser.Username == body.Username && existingUser.Email == body.Email)
            {
                message = _localizer["User with the same username and email already exists."];
            }
            else if (existingUser.Username == body.Username)
            {
                message = _localizer["User with the same username already exists."];
            }
            else if (existingUser.Email == body.Email)
            {
                message = _localizer["User with the same email already exists."];
            }

            return new ErrorMessageResult(message).ToBadRequest();
        }

        // Create user
        var stamp = Guid.NewGuid().ToString();
        var now = DateTimeOffset.UtcNow;
        var user = new User
        {
            Username = body.Username,
            Email = body.Email,
            DisplayName = body.DisplayName,
            PasswordHash = _passwordHashService.HashPassword(body.Password, stamp),
            SecurityStamp = stamp,
            EmailConfirmed = false
        };
        await _dbContext.Users.AddAsync(user, cancellationToken);

        var token = await _securityTokenService.CreateTokenAsync(
            user.EntityId, TokenUsage.AccountVerification, now, cancellationToken);

        token.IsSuccess.Expect(true);

        var emailSendResult = await _emailService.SendEmailAsync(body.Email, new AccountVerification
        {
            User = user,
            Token = token.IfSuccess(x => x.Value).ExpectNotNull()
        });

        if (emailSendResult.IsFailure)
        {
            return new ErrorMessageResult(_localizer["Failed to send account verification email."]).ToBadRequest();
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new OkObjectResult(new UserRegistrationDto
        {
            EmailVerificationRequired = !user.EmailConfirmed
        });
    }
}
