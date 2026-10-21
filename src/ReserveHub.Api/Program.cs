using System.Text.Json;
using System.Text.Json.Serialization;
using ModelContextProtocol;
using ReserveHub.Api.Mcp;
using ReserveHub.Application;
using ReserveHub.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSingleton(TimeProvider.System);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var mcpJsonOptions = new JsonSerializerOptions(McpJsonUtilities.DefaultOptions);
mcpJsonOptions.Converters.Add(new JsonStringEnumConverter());

builder.Services
    .AddMcpServer()
    .WithHttpTransport(options => options.Stateless = true)
    .WithTools<ReserveHubTools>(mcpJsonOptions);

var app = builder.Build();

if (app.Configuration.GetValue<bool>("Database:MigrateOnStartup"))
{
    await app.Services.ApplyMigrationsAsync();
}

app.UseExceptionHandler();

app.MapOpenApi();
app.MapScalarApiReference();
app.MapControllers();
app.MapMcp("/mcp");

app.Run();

public partial class Program;
