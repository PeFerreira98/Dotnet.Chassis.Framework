using PeFerreira98.Chassis.Framework;
using PeFerreira98.Chassis.Framework.Api;
using PeFerreira98.Chassis.Framework.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.SetupDefault();

builder.AddLogging();

builder.AddOpenTelemetry();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();

app.Run();
