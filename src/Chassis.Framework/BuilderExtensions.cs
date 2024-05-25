namespace PeFerreira98.Chassis.Framework;

using Microsoft.AspNetCore.Builder;

public static class BuilderExtensions
{
    public static IHostApplicationBuilder SetupDefault(this IHostApplicationBuilder builder)
    {
        return builder;
    }
}
