using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Interfaces;

namespace UserService.Application.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, Services.UserService>();
            return services;
        }
    }
}
