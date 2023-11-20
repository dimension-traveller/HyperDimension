using Microsoft.Extensions.Hosting;
using Serilog;

namespace HyperDimension.Common.Observability;

public static class ObservabilityConfigurator
{
    public static void UseSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((ctx, conf) =>
        {
            conf.ReadFrom.Configuration(ctx.Configuration);
        });
    }
}
