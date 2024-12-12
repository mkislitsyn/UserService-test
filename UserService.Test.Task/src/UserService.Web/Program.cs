using UserService.Application.Interfaces;
using UserService.DbContexts;
using UserService.Infrastructure.Extensions;
using UserService.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddSignalR();
builder.Services.AddControllers();


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

builder.Services.AddApplicationRepositories(builder.Configuration, connectioDb: connectionString);
builder.Services.AddTransient<IUserService, UserService.Application.Services.UserService>();

var app = builder.Build();


using var scope = app.Services.CreateScope();
using var dbContext = scope.ServiceProvider.GetRequiredService<UserContext>();

await dbContext.Database.EnsureCreatedAsync();

app.UseRouting();

// Map SignalR hub
app.MapHub<UserUpdatesHub>("/userhub");

// Map API controllers
app.MapControllers();

app.Run();