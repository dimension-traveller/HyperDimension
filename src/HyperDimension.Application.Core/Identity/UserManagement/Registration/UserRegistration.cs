﻿using HyperDimension.Application.Common.Interfaces;
using HyperDimension.Application.Common.Interfaces.Identity;
using HyperDimension.Application.Common.Models;
using HyperDimension.Common.Extensions;
using HyperDimension.Common.Options;
using HyperDimension.Common.Utilities;
using HyperDimension.Domain.Email;
using HyperDimension.Domain.Entities.Identity;
using HyperDimension.Domain.Entities.Security;
using HyperDimension.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.UserManagement.Registration;

public class UserRegistration : IRequest<IActionResult>
{
    public string Username { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class UserRegistrationHandler : IRequestHandler<UserRegistration, IActionResult>
{
    private readonly IHyperDimensionDbContext _dbContext;
    private readonly IPasswordHashService _passwordHashService;
    private readonly IEmailService _emailService;
    private readonly IStringLocalizer<UserRegistrationHandler> _localizer;
    private readonly MetadataOptions _metadataOptions;
    private readonly ApplicationOptions _applicationOptions;

    public UserRegistrationHandler(
        IHyperDimensionDbContext dbContext,
        IPasswordHashService passwordHashService,
        IEmailService emailService,
        IStringLocalizer<UserRegistrationHandler> localizer,
        MetadataOptions metadataOptions,
        ApplicationOptions applicationOptions)
    {
        _dbContext = dbContext;
        _passwordHashService = passwordHashService;
        _emailService = emailService;
        _localizer = localizer;
        _metadataOptions = metadataOptions;
        _applicationOptions = applicationOptions;
    }

    public async Task<IActionResult> Handle(UserRegistration request, CancellationToken cancellationToken)
    {
        // Check not exist
        var existingUser = await _dbContext.Users
            .FirstOrDefaultAsync(
                x => x.Email == request.Email || x.Username == request.Username,
                cancellationToken);
        if (existingUser is not null)
        {
            var message = string.Empty;
            if (existingUser.Username == request.Username && existingUser.Email == request.Email)
            {
                message = _localizer["User with the same username and email already exists."];
            }
            else if (existingUser.Username == request.Username)
            {
                message = _localizer["User with the same username already exists."];
            }
            else if (existingUser.Email == request.Email)
            {
                message = _localizer["User with the same email already exists."];
            }

            return new ErrorMessageResult(message).ToBadRequest();
        }

        // Create user
        var stamp = Guid.NewGuid().ToString();
        var verificationToken = RandomUtility.GenerateToken(16);
        var now = DateTimeOffset.UtcNow;
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            DisplayName = request.DisplayName,
            PasswordHash = _passwordHashService.HashPassword(request.Password, stamp),
            SecurityStamp = stamp,
            ConcurrencyStamp = now.GetUnixTimestamp(),
            EmailConfirmed = !_emailService.Enabled
        };
        await _dbContext.Users.AddAsync(user, cancellationToken);

        if (user.EmailConfirmed is false)
        {
            var token = new Token
            {
                Value = verificationToken,
                CreatedAt = now,
                ExpiredAt = now.AddMinutes(30),
                BindTo = user.EntityId,
                Usage = TokenUsage.AccountVerification
            };

            await _dbContext.Tokens.AddAsync(token, cancellationToken);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Send email
        if (user.EmailConfirmed is false)
        {
            await _emailService.SendEmailAsync(request.Email, "Account Verification Email", new AccountVerification
            {
                Username = request.Username,
                DisplayName = request.DisplayName,
                ActivationUrl = _applicationOptions.AccountVerificationUrl.Replace("{TOKEN}", verificationToken),
                SiteName = _metadataOptions.SiteName,
                Token = verificationToken
            });
        }

        return new OkObjectResult(new UserRegistrationDto
        {
            EmailVerificationRequired = !user.EmailConfirmed
        });
    }
}
