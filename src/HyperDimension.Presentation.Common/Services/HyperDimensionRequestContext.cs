using HyperDimension.Application.Common.Extensions;
using HyperDimension.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using OpenTelemetry.Trace;

namespace HyperDimension.Presentation.Common.Services;

public class HyperDimensionRequestContext : IHyperDimensionRequestContext
{
    public string RequestId { get; private set; } = string.Empty;
    public string TraceId { get; private set; } = string.Empty;
    public bool IsAuthenticated { get; private set; }
    public string? AuthenticationSchema { get; set; }
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

        var userId = context.User.GetUserEntityId();
        if (userId is null)
        {
            IsAuthenticated = false;
            return;
        }

        var schema = context.User.Identity.AuthenticationType;

        IsAuthenticated = true;
        AuthenticationSchema = schema;
        UserId = userId.Value;
    }
}
