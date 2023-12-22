using System.Globalization;
using System.Reflection;
using HyperDimension.Common.Configuration;
using HyperDimension.Common.Extensions;
using HyperDimension.Common.Observability.Extensions;
using HyperDimension.Common.Observability.Options;
using HyperDimension.Common.Observability.Options.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;
using Serilog;

namespace HyperDimension.Common.Observability;

public static class ObservabilityConfigurator
{
    public static void AddHyperDimensionObservability(this IServiceCollection services)
    {
        var options = HyperDimensionConfiguration.Instance
            .GetOption<ObservabilityOptions>();

        // Log
        services.AddLogging(loggingBuilder =>
        {
            var loggingOptions = options.Logging;
            loggingBuilder.ClearProviders();
            loggingBuilder.AddLogger(loggingOptions);
        });

        // Trace
        services.AddOpenTelemetry().AddTracing(options);

        // Metrics
        var extraLabels = new Dictionary<string, string>
        {
            { "service_name", options.Name },
            { "service_namespace", options.Namespace },
            { "service_version", options.GetVersion() }
        };
        foreach (var (k, v) in options.Metrics.ExtraLabels)
        {
            extraLabels.TryAdd(k, v);
        }
        Metrics.DefaultRegistry.SetStaticLabels(extraLabels);

        // Health Check
        services.AddHealthChecks().AddCheck("liveness", () => HealthCheckResult.Healthy());
        services.AddHealthChecks().ForwardToPrometheus();
    }

    public static void UseObservability(this IApplicationBuilder app)
    {
        var options = app.ApplicationServices
            .GetRequiredService<ObservabilityOptions>();

        // Liveness
        app.UseHealthChecks("/_debug/health", options.Port, new HealthCheckOptions
        {
            Predicate = r => r.Name == "liveness"
        });

        // Readiness
        app.UseHealthChecks("/_debug/ready", options.Port);

        // Prometheus Metrics
        app.UseMetricServer(options.Port, "/_debug/metrics");
    }

    private static void AddLogger(this ILoggingBuilder loggingBuilder,LoggingOptions loggingOptions)
    {
        var loggerConfiguration = new LoggerConfiguration();

        if (loggingOptions.Console.Enabled)
        {
            loggerConfiguration.WriteTo.Console(
                outputTemplate: loggingOptions.Template,
                formatProvider: CultureInfo.InvariantCulture);
        }

        if (loggingOptions.File.Enabled)
        {
            loggerConfiguration.WriteTo.File(
                outputTemplate: loggingOptions.Template,
                path: loggingOptions.File.Path,
                rollingInterval: loggingOptions.File.Interval,
                formatProvider: CultureInfo.InvariantCulture);
        }

        if (loggingOptions.OpenTelemetry.Enabled)
        {
            loggerConfiguration.WriteTo.OpenTelemetry(configure =>
            {
                configure.Endpoint = loggingOptions.OpenTelemetry.Endpoint;
                configure.Protocol = loggingOptions.OpenTelemetry.Protocol;
            });
        }

        loggerConfiguration.MinimumLevel.Is(loggingOptions.MinimumLevel);
        foreach (var (key, level) in loggingOptions.Override)
        {
            loggerConfiguration.MinimumLevel.Override(key, level);
        }

        Log.Logger = loggerConfiguration.CreateLogger();
        loggingBuilder.AddSerilog(Log.Logger);
    }

    private static void AddTracing(this OpenTelemetryBuilder telemetryBuilder, ObservabilityOptions observabilityOptions)
    {
        var tracingOptions = observabilityOptions.Tracing;

        telemetryBuilder.WithTracing(builder =>
        {
            builder.AddSource(observabilityOptions.Name);

            builder.ConfigureResource(res =>
            {
                res.AddService(
                    observabilityOptions.Name,
                    observabilityOptions.Namespace,
                    observabilityOptions.GetVersion());
            });

            builder.AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.Filter = c =>
                    !(c.IsDebugEndpoint() || c.IsOptionsRequest());
            });

            builder.AddHttpClientInstrumentation(options =>
            {
                options.FilterHttpRequestMessage = request =>
                {
                    var host = request.RequestUri?.Host;
                    if (host is null)
                    {
                        return true;
                    }

                    var collision = tracingOptions.HttpClientFilter.Hosts
                        .FirstOrDefault(x =>
                            host.StartsWith(x, StringComparison.InvariantCultureIgnoreCase));

                    // Collect if no collision (collision is null)
                    return collision is null;
                };
            });

            if (tracingOptions.Console.Enabled)
            {
                builder.AddConsoleExporter(options =>
                {
                    options.Targets = tracingOptions.Console.Target;
                });
            }

            if (tracingOptions.OpenTelemetry.Enabled)
            {
                builder.AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(tracingOptions.OpenTelemetry.Endpoint);
                    options.Protocol = tracingOptions.OpenTelemetry.Protocol;
                });
            }
        });
    }

    private static string GetVersion(this ObservabilityOptions options)
    {
        if (options.Version == "@Version")
        {
            return Assembly.GetEntryAssembly()?.GetName().Version?.ToString(3) ?? "0.0.0";
        }

        return options.Version;
    }
}
