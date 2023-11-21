using System.Net;
using System.Text.Json;
using CleanBlazor.Application.Exceptions;
using CleanBlazor.Shared.Wrapper;
using Microsoft.IdentityModel.Tokens;

namespace CleanBlazor.Server.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = ex switch
                {
                    ApiException => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    SecurityTokenExpiredException => (int)HttpStatusCode.Unauthorized,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                await context.Response.WriteAsJsonAsync(Result.Fail(ex.Message));
                await context.Response.CompleteAsync();
            }
        }
    }
}
