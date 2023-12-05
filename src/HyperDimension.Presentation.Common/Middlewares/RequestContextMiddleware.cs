using HyperDimension.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace HyperDimension.Presentation.Common.Middlewares;

public class RequestContextMiddleware
{
    private readonly RequestDelegate _next;

    public RequestContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context, IHyperDimensionRequestContext requestContext)
    {
        requestContext.SetContext(context);

        return _next(context);
    }
}
