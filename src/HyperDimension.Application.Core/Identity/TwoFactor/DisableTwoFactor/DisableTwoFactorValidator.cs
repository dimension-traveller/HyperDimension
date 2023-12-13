using FluentValidation;
using Microsoft.Extensions.Localization;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace HyperDimension.Application.Core.Identity.TwoFactor.DisableTwoFactor;

public class DisableTwoFactorValidator : AbstractValidator<DisableTwoFactor>
{
    private static readonly string[] AvailableTwoFactorTypes = ["totp", "email"];

    public DisableTwoFactorValidator(IStringLocalizer<DisableTwoFactorValidator> localizer)
    {
        RuleFor(x => x.Body.Type)
            .Must(x => AvailableTwoFactorTypes.Contains(x))
            .WithMessage(localizer["Unknown two factor authentication type."]);
    }
}

