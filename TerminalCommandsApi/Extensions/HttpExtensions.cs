using System.Linq;
using Microsoft.AspNetCore.Http;

namespace TerminalCommandsApi.Extensions
{
    public static class HttpExtensions
    {
        public static string GetUserId(this HttpContext context)
        {
            return context.User.Claims.Single(claim => claim.Type == "Id").Value;
        }

    }
}