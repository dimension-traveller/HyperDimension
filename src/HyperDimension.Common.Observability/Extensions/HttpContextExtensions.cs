using Microsoft.AspNetCore.Http;

namespace HyperDimension.Common.Observability.Extensions;

public static class HttpContextExtensions
{
    public static bool IsDebugEndpoint(this HttpContext context)
    {
        return context.Request.Path.StartsWithSegments("/_debug");
    }

    public static bool IsOptionsRequest(this HttpContext context)
    {
        return context.Request.Method == HttpMethods.Options;
    }
}
