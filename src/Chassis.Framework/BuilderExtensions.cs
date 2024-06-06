using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace PeFerreira98.Chassis.Framework;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder SetupDefault(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        return builder;
    }

    public static WebApplication SetupDefault(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.MapHealthChecks("/alive", new HealthCheckOptions { Predicate = r => r.Tags.Contains("live") });

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        return app;
    }
}
