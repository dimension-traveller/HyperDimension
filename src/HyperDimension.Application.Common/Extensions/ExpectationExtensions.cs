using System.Runtime.CompilerServices;
using HyperDimension.Application.Common.Exceptions;

namespace HyperDimension.Application.Common.Extensions;

public static class ExpectationExtensions
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

    public static T Expect<T>(this T value, T expectedValue,
        [CallerArgumentExpression(nameof(value))] string callerArg = "UNKNOWN",
        [CallerMemberName] string callerMember = "UNKNOWN") where T : struct
    {
        if (value.Equals(expectedValue))
        {
            return value;
        }

        throw new UnexpectedException(
            $"Unexpected value {value} for argument expression {callerArg} from {callerMember}, expect {expectedValue}");
    }

    public static async Task<T> Expect<T>(this Task<T> value, T expectedValue,
        [CallerArgumentExpression(nameof(value))] string callerArg = "UNKNOWN",
        [CallerMemberName] string callerMember = "UNKNOWN") where T : struct
    {
        var taskResult = await value;

        if (taskResult.Equals(expectedValue))
        {
            return taskResult;
        }

        throw new UnexpectedException(
            $"Unexpected value {taskResult} for argument expression {callerArg} from {callerMember}, expect {expectedValue}");
    }
}
