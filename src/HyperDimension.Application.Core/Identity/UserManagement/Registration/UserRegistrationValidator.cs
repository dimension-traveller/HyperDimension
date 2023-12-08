using FluentValidation;
using HyperDimension.Application.Common.Extensions;

namespace HyperDimension.Application.Core.Identity.UserManagement.Registration;

public class UserRegistrationValidator : AbstractValidator<UserRegistration>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.Username)
            .MinimumLength(3)
            .MaximumLength(16);

        RuleFor(x => x.DisplayName)
            .MinimumLength(3)
            .MaximumLength(32);

        RuleFor(x => x.Email)
            .Empty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .Password();
    }
}
