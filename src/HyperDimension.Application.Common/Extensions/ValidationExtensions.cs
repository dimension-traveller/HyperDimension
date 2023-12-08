using System.Text.RegularExpressions;
using FluentValidation;

namespace HyperDimension.Application.Common.Extensions;

public static partial class ValidationExtensions
{
    [GeneratedRegex(@"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&-+=()])(?=\\\\S+$).{8, 32}$")]
    private static partial Regex ValidatePassword();

    public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(ValidatePassword().IsMatch);
    }
}
