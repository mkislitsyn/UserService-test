using UserService.Application.Extensions;
using UserService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services
 builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

builder.Services.AddInfrastructureServices(builder.Configuration, connectioDb: connectionString);
builder.Services.AddApplicationServices();

var app = builder.Build();

//app.UseDefaultFiles();
//app.UseStaticFiles();

using var scope = app.Services.CreateScope();
await scope.AddInfrastructureScopesAsync();

app.UseWebSockets();

// Map API controllers
app.MapControllers();

app.Run();