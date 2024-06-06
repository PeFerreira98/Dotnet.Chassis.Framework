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
builder.Services.AddOAuthHttpClient<IOAuthMockClient>();

var app = builder.Build();
app.SetupDefault();
app.AddPrometheusEndpoints(builder.Configuration);

app.MapEndpoints();

app.Run();

// Integration Test only
public partial class Program { }