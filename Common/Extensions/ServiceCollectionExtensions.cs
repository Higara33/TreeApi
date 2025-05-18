using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPostgreDb<TContext>(this IServiceCollection services, IConfiguration config) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options =>
            options.UseNpgsql(config.GetConnectionString("Default")));
            return services;
        }
    }
}
