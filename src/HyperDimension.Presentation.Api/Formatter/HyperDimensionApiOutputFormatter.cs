using System.Text;
using HyperDimension.Application.Common.Models;
using HyperDimension.Presentation.Api.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using OpenTelemetry.Trace;

namespace HyperDimension.Presentation.Api.Formatter;

public class HyperDimensionApiOutputFormatter : TextOutputFormatter
{
    public HyperDimensionApiOutputFormatter()
    {
        SupportedMediaTypes.Add("application/json");
        SupportedEncodings.Add(Encoding.UTF8);
    }

    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var httpContext = context.HttpContext;
        var tracerId = Tracer.CurrentSpan.Context.TraceId.ToHexString();

        var response = new HyperDimensionApiResponse
        {
            TraceId = tracerId
        };

        if (context.Object is ErrorMessageResult emr)
        {
            response.Message = emr.Message;
        }
        else
        {
            response.Data = context.Object ?? new { };
        }

        httpContext.Response.Headers.TryAdd("X-Trace-Id", tracerId);
        await httpContext.Response.WriteAsJsonAsync(response);
    }
}
