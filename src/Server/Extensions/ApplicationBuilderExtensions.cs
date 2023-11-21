using System.Globalization;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Application.Configuration;
using CleanBlazor.Server.Configuration;
using CleanBlazor.Server.Hubs;
using CleanBlazor.Server.Middlewares;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Localization;
using Microsoft.AspNetCore.Localization;
using Serilog;

namespace CleanBlazor.Server.Extensions;

internal static class ApplicationBuilderExtensions
{
    internal static IApplicationBuilder UseExceptionHandling(
        this IApplicationBuilder app,
        IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseWebAssemblyDebugging();
        }

        return app;
    }

    internal static IApplicationBuilder UseForwarding(this IApplicationBuilder app, IConfiguration configuration)
    {
        var config = configuration.GetSection(nameof(AppConfiguration)).Get<AppConfiguration>();
        if (config.BehindSslProxy)
        {
            app.UseCors();
            app.UseForwardedHeaders();
        }

        return app;
    }

    internal static void ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", typeof(Program).Assembly.GetName().Name);
            options.RoutePrefix = "swagger";
            options.DisplayRequestDuration();
        });
    }

    internal static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
        => app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
            endpoints.MapFallbackToFile("index.html");
            endpoints.MapHub<SignalRHub>(ApplicationConstants.SignalR.HubUrl);
        });

    internal static IApplicationBuilder UseServerCultureFromPreferences(
        this IApplicationBuilder app,
        IServiceProvider serviceProvider)
    {
        var storageService = serviceProvider.GetService<ServerPreferenceManager>();
        if (storageService != null)
        {
            // TODO - should implement ServerStorageProvider to work correctly!
            CultureInfo culture;
            if (storageService.GetPreference().GetAwaiter().GetResult() is ServerPreference preference)
            {
                culture = new CultureInfo(preference.LanguageCode);
            }
            else
            {
                culture = new CultureInfo(LocalizationConstants.DefaultLanguage.Code);
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            Log.Information("Configured {Culture} as server culture", culture.Name);
        }

        return app;
    }

    internal static IApplicationBuilder UseRequestLocalizationByCulture(this IApplicationBuilder app)
    {
        CultureInfo[] supportedCultures =
            LocalizationConstants.SupportedLanguages.Select(l => new CultureInfo(l.Code)).ToArray();

        app.UseRequestLocalization(options =>
        {
            options.SupportedUICultures = supportedCultures;
            options.SupportedCultures = supportedCultures;
            options.DefaultRequestCulture = new RequestCulture(LocalizationConstants.DefaultLanguage.Code);
            options.ApplyCurrentCultureToResponseHeaders = true;
        });

        app.UseMiddleware<RequestCultureMiddleware>();

        return app;
    }
}
