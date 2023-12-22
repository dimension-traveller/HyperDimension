using OpenTelemetry.Exporter;

namespace HyperDimension.Common.Observability.Options.Logging;

public class TracingOptions
{
    public HttpClientFilterOptions HttpClientFilter { get; set; } = new();

    public ConsoleExporterOptions Console { get; set; } = new();

    public OpenTelemetryExporterOptions OpenTelemetry { get; set; } = new();
}

public class ConsoleExporterOptions
{
    public bool Enabled { get; set; }

    public ConsoleExporterOutputTargets Target { get; set; } = ConsoleExporterOutputTargets.Console;
}

public class OpenTelemetryExporterOptions
{
    public bool Enabled { get; set; }

    public string Endpoint { get; set; } = "http://127.0.0.1:4318";

    public OtlpExportProtocol Protocol { get; set; } = OtlpExportProtocol.Grpc;
}

public class HttpClientFilterOptions
{
    public List<string> Hosts { get; set; } = [];
}
