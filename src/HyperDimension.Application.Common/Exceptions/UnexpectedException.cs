namespace HyperDimension.Application.Common.Exceptions;

public class UnexpectedException : Exception
{
    public new string Message { get; }

    public UnexpectedException(string message)
    {
        Message = message;
    }
}
