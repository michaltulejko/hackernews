using HackerNewsASP.Models.Exceptions;

namespace HackerNewsASP.Middleware;

public class GlobalExceptionMiddleware(RequestDelegate next, IHostEnvironment env)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (HandledException exception)
        {
            context.Response.StatusCode = (int)exception.StatusCode;
            context.Response.ContentType = "application/json";

            var errorPayload = new
            {
                Error = exception.Message
            };

            await context.Response.WriteAsJsonAsync(errorPayload);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var errorPayload = new
            {
                Error = "An unhandled exception occurred.",
                Message = env.IsDevelopment() ? exception.StackTrace : exception.Message
            };

            await context.Response.WriteAsJsonAsync(errorPayload);
        }
    }
}