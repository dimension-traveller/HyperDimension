using FluentValidation;
using HyperDimension.Application.Common.Extensions;
using HyperDimension.Common.Extensions;
using Microsoft.Extensions.Localization;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace HyperDimension.Application.Core.Identity.UserManagement.PasswordReset;

public class PasswordResetValidator : AbstractValidator<PasswordReset>
{
    public PasswordResetValidator(IStringLocalizer<PasswordResetValidator> localizer)
    {
        RuleFor(x => x.Body.Password)
            .Password()
            .WithMessage(localizer["{0} is invalid"].Format(localizer["Password"]));
    }
}

