namespace HyperDimension.Common;

public readonly struct Result<T>
{
    private T? Value { get; init; }
    private string ErrorMessage { get; init; }
    private bool Ok { get; init; }

    public static Result<T> Success(T value) => new()
    {
        Value = value, ErrorMessage = string.Empty, Ok = true
    };

    public static Result<T> Failure(string errorMessage) => new()
    {
        Value = default, ErrorMessage = errorMessage, Ok = false
    };

    public bool IsSuccess => Ok;
    public bool IsFailure => !Ok;

    public TReturn Match<TReturn>(Func<T, TReturn> success, Func<string, TReturn> failure)
    {
        return Ok ? success(Value!) : failure(ErrorMessage);
    }

    public Task<TReturn> MatchAsync<TReturn>(Func<T, Task<TReturn>> success, Func<string, Task<TReturn>> failure)
    {
        return Ok ? success(Value!) : failure(ErrorMessage);
    }

    public TReturn? IfSuccess<TReturn>(Func<T, TReturn> success)
    {
        return Ok ? success(Value!) : default;
    }

    public async Task<TReturn?> IfSuccessAsync<TReturn>(Func<T, Task<TReturn>> success)
    {
        return Ok ? await success(Value!) : default;
    }

    public TReturn? IfFailure<TReturn>(Func<string, TReturn> failure)
    {
        return Ok ? default : failure(ErrorMessage);
    }

    public async Task<TReturn?> IfFailureAsync<TReturn>(Func<string, Task<TReturn>> failure)
    {
        return Ok ? default : await failure(ErrorMessage);
    }

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(string errorMessage) => Failure(errorMessage);
}
