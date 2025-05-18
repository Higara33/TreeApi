using Common.Models;
using Common.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Common.Middleware
{
    public class ExceptionLoggingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public ExceptionLoggingMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (SecureException secEx)
            {
                var journalRepo = context.RequestServices.GetRequiredService<IJournalRepository>();
                var entry = await journalRepo.LogExceptionAsync(context, secEx);

                var result = new
                {
                    type = secEx.GetType().Name,
                    id = entry.EventId,
                    data = new { message = secEx.Message }
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
            catch (Exception ex)
            {
                var journalRepo = context.RequestServices.GetRequiredService<IJournalRepository>();
                var entry = await journalRepo.LogExceptionAsync(context, ex);

                var result = new
                {
                    type = "Exception",
                    id = entry.EventId,
                    data = new { message = $"Internal server error ID = {entry.EventId}" }
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(result));
            }
        }
    }
}
