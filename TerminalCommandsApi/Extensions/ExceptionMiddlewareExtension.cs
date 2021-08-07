using Microsoft.AspNetCore.Builder;
using TerminalCommandsApi.Middlewares;

namespace TerminalCommandsApi.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureCommanderExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

    }
}