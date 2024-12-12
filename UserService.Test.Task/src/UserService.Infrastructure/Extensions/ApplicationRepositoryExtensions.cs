using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DbContexts;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Repositories;

namespace UserService.Infrastructure.Extensions
{
    public static class ApplicationRepositoryExtensions
    {
        public static IServiceCollection AddApplicationRepositories(this IServiceCollection services, IConfiguration config, string connectioDb)
        {        

            services.AddDbContext<UserContext>(options => options.UseSqlServer(connectioDb));
            
            services.AddTransient<IUserRepository, UserRepository>();

            return services;
        }
    }
}
