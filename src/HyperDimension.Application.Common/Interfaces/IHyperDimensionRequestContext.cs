using Microsoft.AspNetCore.Http;

namespace HyperDimension.Application.Common.Interfaces;

public interface IHyperDimensionRequestContext
{
    public string RequestId { get; }

    public string TraceId { get; }

    public bool IsAuthenticated { get; }

    public Guid? UserId { get; }

    public void SetContext(HttpContext context);
}
