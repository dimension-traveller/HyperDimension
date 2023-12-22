using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;

namespace HyperDimension.Common.Observability.Options.Logging;

public class LoggingOptions
{
    public string Template { get; set; } = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

    public LogEventLevel MinimumLevel { get; set; } = LogEventLevel.Information;

    public Dictionary<string, LogEventLevel> Override { get; set; } = new()
    {
        { "Microsoft", LogEventLevel.Warning },
        { "Microsoft.Hosting.Lifetime", LogEventLevel.Information },
        { "Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning }
    };

    public ConsoleOptions Console { get; set; } = new();

    public FileOptions File { get; set; } = new();

    public OpenTelemetryOptions OpenTelemetry { get; set; } = new();
}

public class ConsoleOptions
{
    public bool Enabled { get; set; }
}

public class FileOptions
{
    public bool Enabled { get; set; }

    public string Path { get; set; } = "./data/logs/hd-.log";

    public RollingInterval Interval { get; set; } = RollingInterval.Day;
}

public class OpenTelemetryOptions
{
    public bool Enabled { get; set; }

    public string Endpoint { get; set; } = "http://127.0.0.1:4318/v1/logs";

    public OtlpProtocol Protocol { get; set; } = OtlpProtocol.HttpProtobuf;
}
