using System.Globalization;
using Blazored.LocalStorage;
using CleanBlazor.Client.Authentication;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Permission;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

namespace CleanBlazor.Client.Extensions;

public static class WebAssemblyHostBuilderExtensions
{
    public static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
    {
        builder.RootComponents.Add<App>("#app");

        return builder;
    }

    public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
    {
        builder
            .AddAuth()
            .AddHttp();

        builder
            .Services
            .AddLocalization(options => { options.ResourcesPath = Path.Combine("Configuration", "Resources"); })
            .AddBlazoredLocalStorage()
            .AddMudServices(configuration =>
            {
                configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                configuration.SnackbarConfiguration.HideTransitionDuration = 100;
                configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
                configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
                configuration.SnackbarConfiguration.ShowCloseIcon = false;
            })
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .AddSingleton(TimeProvider.System)
            .AddScoped<ClientPreferenceManager>()
            .AddScoped<ApplicationStateProvider>()
            .AddScoped<AuthenticationStateProvider, ApplicationStateProvider>();

        return builder;
    }

    private static WebAssemblyHostBuilder AddAuth(this WebAssemblyHostBuilder builder)
    {
        builder.Services
            .AddTransient<AuthenticationDelegatingHandler>()
            .AddTransient<AuthenticationManager>()
            .AddAuthorizationCore(options =>
            {
                Permissions.GetRegisteredPermissions().ForEach(permission => options.AddPolicy(permission,
                    policy => policy.RequireClaim(ApplicationClaimTypes.Permission, permission)));
            });

        return builder;
    }

    private static WebAssemblyHostBuilder AddHttp(this WebAssemblyHostBuilder builder)
    {
        builder.Services
            .AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(ApplicationConstants.HttpClient.ClientName))
            .AddHttpClient(ApplicationConstants.HttpClient.ClientName,
                client =>
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Clear();
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture
                        ?.TwoLetterISOLanguageName);
                    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
                })
            .AddHttpMessageHandler<AuthenticationDelegatingHandler>();

        return builder;
    }
}
