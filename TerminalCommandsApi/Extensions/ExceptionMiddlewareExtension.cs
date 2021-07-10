using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using TerminalCommands.Middlewares;

namespace TerminalCommands.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void ConfigureCommanderExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }

    }
}