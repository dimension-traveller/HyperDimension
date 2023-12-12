using System.Diagnostics.CodeAnalysis;
using HyperDimension.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HyperDimension.Application.Common.Behavior;

[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IActionResult
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = request.GetType().Name;
            _logger.LogError(e, "Unhandled exception while handling {RequestName}", requestName);
            return (TResponse)(IActionResult)new ObjectResult(new ErrorMessageResult("Internal Server Error"))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
