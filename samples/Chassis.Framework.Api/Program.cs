using PeFerreira98.Chassis.Framework;
using PeFerreira98.Chassis.Framework.Api;
using PeFerreira98.Chassis.Framework.Api.Clients;
using PeFerreira98.Chassis.Framework.Http;
using PeFerreira98.Chassis.Framework.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.SetupDefault();

builder.AddLogging();

builder.AddOpenTelemetry();

builder.ConfigureClients();
builder.Services.AddNoAuthHttpClient<IMockClient>();

var app = builder.Build();
app.SetupDefault();

app.MapEndpoints();

app.Run();