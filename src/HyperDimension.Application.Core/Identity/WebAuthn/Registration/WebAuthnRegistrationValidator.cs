﻿using FluentValidation;
using HyperDimension.Common.Extensions;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.WebAuthn.Registration;

public class WebAuthnRegistrationValidator : AbstractValidator<WebAuthnRegistration>
{
    public WebAuthnRegistrationValidator(IStringLocalizer<WebAuthnRegistrationValidator> localizer)
    {
        RuleFor(x => x.Body.Username)
            .MinimumLength(3)
            .WithMessage(
                localizer["{0} must be between {1} and {2} characters long"].Format(localizer["Username"], 3, 16))
            .MaximumLength(16)
            .WithMessage(
                localizer["{0} must be between {1} and {2} characters long"].Format(localizer["Username"], 3, 16));

        RuleFor(x => x.Body.DisplayName)
            .MinimumLength(3)
            .WithMessage(
                localizer["{0} must be between {1} and {2} characters long"].Format(localizer["Display Name"], 3, 32))
            .MaximumLength(32)
            .WithMessage(
                localizer["{0} must be between {1} and {2} characters long"].Format(localizer["Display Name"], 3, 32));

        RuleFor(x => x.Body.Email)
            .Empty()
            .WithMessage(localizer["{0} should not be empty"].Format(localizer["Email"]))
            .EmailAddress()
            .WithMessage(localizer["{0} is invalid"].Format(localizer["Email"]));
    }
}
