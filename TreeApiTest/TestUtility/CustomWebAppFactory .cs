using AuthService;
using AuthService.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TreeApiTest.TestUtility
{
    public class CustomWebAppFactory<TProgram, TContext> : WebApplicationFactory<TProgram>
        where TProgram : class
        where TContext : DbContext
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Убираем старую регистрацию
                var desc = services.SingleOrDefault(d =>
                  d.ServiceType == typeof(DbContextOptions<TContext>));
                if (desc != null) services.Remove(desc);

                // Регистрируем InMemory для нужного контекста
                services.AddDbContext<TContext>(opts =>
                  opts.UseInMemoryDatabase(typeof(TContext).Name));
            });
        }
    }
}
