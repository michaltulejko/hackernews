using HackerNewsASP.Middleware;

namespace HackerNewsASP.Extensions;

public static class MiddlewareExtensions
{
    public static void ConfigureMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}