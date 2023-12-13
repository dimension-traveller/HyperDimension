using FluentValidation;
using HyperDimension.Common.Extensions;
using Microsoft.Extensions.Localization;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace HyperDimension.Application.Core.Identity.UserManagement.PasswordResetRequest;

public class PasswordResetRequestValidator : AbstractValidator<PasswordResetRequest>
{
    public PasswordResetRequestValidator(IStringLocalizer<PasswordResetRequestValidator> localizer)
    {
        RuleFor(x => x.Body.Email)
            .Empty()
            .WithMessage(localizer["{0} should not be empty"].Format(localizer["Email"]))
            .EmailAddress()
            .WithMessage(localizer["{0} is invalid"].Format(localizer["Email"]));
    }
}
