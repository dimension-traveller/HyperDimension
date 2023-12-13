using Microsoft.AspNetCore.Mvc;

namespace HyperDimension.Application.Common.Models;

public class ErrorMessageResult
{
    public string Message { get; set; } = string.Empty;

    public ErrorMessageResult()
    {
    }

    public ErrorMessageResult(string message)
    {
        Message = message;
    }

    public BadRequestObjectResult ToBadRequest() => new(this);

    public NotFoundObjectResult ToNotFound() => new(this);
}
