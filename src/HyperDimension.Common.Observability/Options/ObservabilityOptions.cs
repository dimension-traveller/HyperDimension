using HyperDimension.Common.Attributes;
using HyperDimension.Common.Observability.Options.Logging;

namespace HyperDimension.Common.Observability.Options;

[OptionSection("Observability")]
public class ObservabilityOptions
{
    public string Name { get; set; } = "HyperDimension";

    public string Namespace { get; set; } = "production";

    public string Version { get; set; } = "@Version";

    public int Port { get; set; } = 3200;

    public LoggingOptions Logging { get; set; } = new();

    public TracingOptions Tracing { get; set; } = new();

    public MetricsOptions Metrics { get; set; } = new();
}
