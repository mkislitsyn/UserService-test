using UserService.Application.Extensions;
using UserService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
 builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

builder.Services.AddInfrastructureServices(builder.Configuration, connectioDb: connectionString);
builder.Services.AddApplicationServices();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHealthChecks();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

using var scope = app.Services.CreateScope();
await scope.AddInfrastructureScopesAsync();

app.UseWebSockets();
app.UseHttpsRedirection();
app.MapGet("/", async context =>
{
    await context.Response.WriteAsync("Welcome to the default startup page!");
});

app.MapGet("/health", () => Results.Ok("API is healthy!"));
// Map API controllers
app.MapControllers();

app.Run();