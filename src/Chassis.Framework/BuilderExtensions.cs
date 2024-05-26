using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace PeFerreira98.Chassis.Framework;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder SetupDefault(this IHostApplicationBuilder builder)
    {
        //TODO: implement

        builder.AddDefaultHealthChecks();

        return builder;
    }

    private static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
    {
        // Add a default liveness check to ensure app is responsive
        builder.Services.AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        return builder;
    }
}
