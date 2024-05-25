using PeFerreira98.Chassis.Framework;

var builder = WebApplication.CreateBuilder(args);

//builder.SetupDefault();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
