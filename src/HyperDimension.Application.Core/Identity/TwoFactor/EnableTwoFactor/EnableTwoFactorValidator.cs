using FluentValidation;
using Microsoft.Extensions.Localization;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace HyperDimension.Application.Core.Identity.TwoFactor.EnableTwoFactor;

public class EnableTwoFactorValidator : AbstractValidator<EnableTwoFactor>
{
    private static readonly string[] AvailableTwoFactorTypes = ["totp", "email"];

    public EnableTwoFactorValidator(IStringLocalizer<EnableTwoFactorValidator> localizer)
    {
        RuleFor(x => x.Body.Type)
            .Must(x => AvailableTwoFactorTypes.Contains(x))
            .WithMessage(localizer["Unknown two factor authentication type."]);
    }
}

