using System.Globalization;
using Microsoft.Extensions.Primitives;

namespace CleanBlazor.Server.Middlewares;

public class RequestCultureMiddleware
{
    private readonly RequestDelegate _next;

    public RequestCultureMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        StringValues cultureQuery = context.Request.Query["culture"];
        if (!string.IsNullOrWhiteSpace(cultureQuery))
        {
            var culture = new CultureInfo(cultureQuery);

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }
        else if (context.Request.Headers.TryGetValue("Accept-Language", out StringValues header) && header.Any())
        {
            var culture = new CultureInfo(header[0]!.Split(',')[0].Trim());

            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
        }

        await _next(context);
    }
}
