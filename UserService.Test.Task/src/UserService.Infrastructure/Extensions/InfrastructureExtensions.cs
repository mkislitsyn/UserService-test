using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DbContexts;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.Extensions
{
    public static class InfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config, string connectioDb)
        {
            services.AddDbContext<UserContext>(options => options.UseSqlServer(connectioDb));
            services.AddTransient<IUserRepository, UserRepository>();
            return services;
        }

        public static async Task<IServiceScope> AddInfrastructureScopesAsync(this IServiceScope scope)
        {
            using var dbContext = scope.ServiceProvider.GetRequiredService<UserContext>();
            await dbContext.Database.EnsureCreatedAsync();
            return scope;
        }
    }
}
