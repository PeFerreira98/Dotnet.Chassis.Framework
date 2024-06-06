using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace PeFerreira98.Chassis.Framework.Logging;

public static class OpenTelemetryExtensions
{
    public static IHostApplicationBuilder AddOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.IncludeFormattedMessage = true;
            logging.IncludeScopes = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddBuiltInMeters();

                if (UsePrometheusExporter(builder.Configuration))
                {
                    metrics.AddPrometheusExporter();
                }
            })
            .WithTracing(tracing =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    // We want to view all traces in development
                    tracing.SetSampler(new AlwaysOnSampler());
                }

                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();
            });

        if (UseOtlpExporter(builder.Configuration))
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }

        return builder;
    }


    public static WebApplication AddPrometheusEndpoints(this WebApplication app, IConfiguration configuration)
    {
        if (UsePrometheusExporter(configuration))
        {
            string exporterPath = configuration.GetValue<string>("PROMETHEUS_METRICS_PATH");
            app.UseOpenTelemetryPrometheusScrapingEndpoint(context => context.Request.Path == exporterPath);
        }

        return app;
    }

    private static MeterProviderBuilder AddBuiltInMeters(this MeterProviderBuilder meterProviderBuilder) =>
        meterProviderBuilder.AddMeter(
            "Microsoft.AspNetCore.Hosting",
            "Microsoft.AspNetCore.Server.Kestrel",
            "System.Net.Http",
            "System.Net.Sockets",
            "System.Net.NameResolution",
            "System.Net.Security");

    private static bool UsePrometheusExporter(IConfiguration configuration) => !string.IsNullOrWhiteSpace(configuration["PROMETHEUS_METRICS_PATH"]);

    private static bool UseOtlpExporter(IConfiguration configuration) => !string.IsNullOrWhiteSpace(configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
}
