using System.Runtime.CompilerServices;
using HyperDimension.Application.Common.Exceptions;

namespace HyperDimension.Application.Common.Extensions;

public static class NullableExtensions
{
    public static T ExpectNotNull<T>(this T? value,
        [CallerArgumentExpression(nameof(value))] string callerArg = "UNKNOWN",
        [CallerMemberName] string callerMember = "UNKNOWN") where T : class
    {
        return value ?? throw new UnexpectedException(
            $"Unexpected null value for argument expression {callerArg} from {callerMember}");
    }

    public static T ExpectNotNull<T>(this T? value,
        [CallerArgumentExpression(nameof(value))] string callerArg = "UNKNOWN",
        [CallerMemberName] string callerMember = "UNKNOWN") where T : struct
    {
        if (value.HasValue is false)
        {
            throw new UnexpectedException($"Unexpected null value for argument expression {callerArg} from {callerMember}");
        }

        return value.Value;
    }

    public static async Task<T> ExpectNotNull<T>(this Task<T?> value,
        [CallerArgumentExpression(nameof(value))] string callerArg = "UNKNOWN",
        [CallerMemberName] string callerMember = "UNKNOWN") where T : class
    {
        var taskResult = await value;

        return taskResult ?? throw new UnexpectedException(
            $"Unexpected null value for argument expression {callerArg} from {callerMember}");
    }

    public static async Task<T> ExpectNotNull<T>(this Task<T?> value,
        [CallerArgumentExpression(nameof(value))] string callerArg = "UNKNOWN",
        [CallerMemberName] string callerMember = "UNKNOWN") where T : struct
    {
        var taskResult = await value;

        if (taskResult.HasValue is false)
        {
            throw new UnexpectedException($"Unexpected null value for argument expression {callerArg} from {callerMember}");
        }

        return taskResult.Value;
    }
}
