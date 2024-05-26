using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PeFerreira98.Chassis.Framework.Logging.LogEnricher;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.Grafana.Loki;

namespace PeFerreira98.Chassis.Framework.Logging;

public static class LoggingExtensions
{
    public static IHostApplicationBuilder AddLogging(this IHostApplicationBuilder builder)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .Enrich.WithTracingContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:yy-MM-dd HH:mm:ss} {Level:u3} {TraceId}] {Message:lj}{NewLine}{Exception}");

        if (UseLokiExporter(builder.Configuration))
        {
            loggerConfiguration.AddLokiExporter(builder);
        }

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(loggerConfiguration.CreateLogger());

        return builder;
    }

    private static LoggerConfiguration WithTracingContext(this LoggerEnrichmentConfiguration enrich)
    {
        if (enrich == null)
        {
            throw new ArgumentNullException(nameof(enrich));
        }

        return enrich.With<TraceContextLogEventEnricher>();
    }

    private static void AddLokiExporter(this LoggerConfiguration loggerConfiguration, IHostApplicationBuilder builder)
    {
        string appName = builder.Configuration.GetValue<string>("OTEL_SERVICE_NAME");
        string lokiUri = builder.Configuration.GetValue<string>("LOGS_EXPORTER_LOKI_ENDPOINT");
        var lokiLabels = new[] { new LokiLabel() { Key = "app", Value = appName } };

        loggerConfiguration.WriteTo.GrafanaLoki(lokiUri, lokiLabels);
    }

    private static bool UseLokiExporter(IConfiguration configuration) => !string.IsNullOrWhiteSpace(configuration["LOGS_EXPORTER_LOKI_ENDPOINT"]);
}
