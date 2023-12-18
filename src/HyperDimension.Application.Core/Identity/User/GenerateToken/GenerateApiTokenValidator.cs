using FluentValidation;
using HyperDimension.Common.Extensions;
using Microsoft.Extensions.Localization;

namespace HyperDimension.Application.Core.Identity.User.GenerateToken;

public class GenerateApiTokenValidator : AbstractValidator<GenerateApiToken>
{
    public GenerateApiTokenValidator(IStringLocalizer<GenerateApiTokenValidator> localizer)
    {
        RuleFor(x => x.Body.Name)
            .MinimumLength(3)
            .WithMessage(
                localizer["{0} must be between {1} and {2} characters long"].Format(localizer["Api Token"], 3, 16))
            .MaximumLength(16)
            .WithMessage(
                localizer["{0} must be between {1} and {2} characters long"].Format(localizer["Api Token"], 3, 16));

        RuleFor(x => x.Body.ValidFor)
            .GreaterThanOrEqualTo(0)
            .WithMessage(localizer["{0} must be greater than or equal to {1} and less than or equal to {2}."].Format(localizer["Api Token Valid For"], 0, 365))
            .LessThanOrEqualTo(365)
            .WithMessage(localizer["{0} must be greater than or equal to {1} and less than or equal to {2}."].Format(localizer["Api Token Valid For"], 0, 365));
    }
}
