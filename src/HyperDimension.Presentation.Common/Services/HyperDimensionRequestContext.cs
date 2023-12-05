using HyperDimension.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Trace;

namespace HyperDimension.Presentation.Common.Services;

public class HyperDimensionRequestContext : IHyperDimensionRequestContext
{
    public string RequestId { get; private set; } = string.Empty;
    public string TraceId { get; private set; } = string.Empty;
    public bool IsAuthenticated { get; private set; }
    public Guid? UserId { get; private set; }

    public void SetContext(HttpContext context)
    {
        RequestId = context.TraceIdentifier;
        TraceId = Tracer.CurrentSpan.Context.TraceId.ToHexString();

        if (context.User.Identity?.IsAuthenticated is null or false)
        {
            IsAuthenticated = false;
            return;
        }

        var userId = context.User.FindFirst("sub")?.Value;
        var isValidGui = Guid.TryParse(userId, out var guid);
        if (isValidGui is false)
        {
            IsAuthenticated = false;
            return;
        }

        IsAuthenticated = true;
        UserId = guid;
    }
}
