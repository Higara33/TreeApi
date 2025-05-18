using Common.Extensions;
using Common.Middleware;
using Common.Repositories;
using Common.Utilities;
using JournalService.Data;
using JournalService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IJournalRepository, JournalRepository>();
builder.Services.AddSingleton<ITimeProvider, TimeProvider>();

builder.Services.AddPostgreDb<JournalDbContext>(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ExceptionLoggingMiddleware>();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
