﻿using PeFerreira98.Chassis.Framework.Api.Clients;

namespace PeFerreira98.Chassis.Framework.Api;

public static class ApiEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/logs", (ILogger<Program> logger) =>
        {
            logger.LogInformation("Test INF");
            logger.LogWarning("Test WRN");
            logger.LogError("Test ERR");
        });

        endpoints.MapGet("/mock", async (IMockClient client) =>
            (await client.GetAsync()).Content);

        endpoints.MapGet("/oauthmock", async (IOAuthMockClient client) =>
            (await client.GetAsync()).Content);
    }
}
