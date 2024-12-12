using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Repositories;
using UserService.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddTransient<IUserService, UserService.Application.Services.UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

var app = builder.Build();

app.UseRouting();

// Map SignalR hub
app.MapHub<UserUpdatesHub>("/userhub");

// Map API controllers
app.MapControllers();

app.Run();