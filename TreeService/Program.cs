using Common.Extensions;
using Common.Middleware;
using Common.Repositories;
using TreeService.Data;
using TreeService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ITreeRepository, TreeRepository>();

builder.Services.AddPostgreDb<TreeContext>(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggingMiddleware>();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

namespace TreeService
{
    // Этот класс нужен для WebApplicationFactory<TEntryPoint>
    public partial class Program { }
}