using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Authentication;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Identity.Account;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Identity.Authentication;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Identity.Roles;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Preferences;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Net.Http;

namespace BlazorHero.CleanArchitecture.Client.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        private const string ClientName = "BlazorHero.API";

        public static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("#app");

            return builder;
        }

        public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {
            builder
                .Services
                .AddAuthorizationCore()
                .AddBlazoredLocalStorage()
                .AddLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                })
                .AddMudServices(
                configuration =>
                {
                    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                    configuration.SnackbarConfiguration.HideTransitionDuration = 100;
                    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
                    configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
                    configuration.SnackbarConfiguration.ShowCloseIcon = false;
                })
                .AddScoped<PreferenceService>()
                .AddScoped<BlazorHeroStateProvider>()
                .AddScoped<AuthenticationStateProvider, BlazorHeroStateProvider>()
                .AddScoped(sp => sp
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient(ClientName))
                .AddTransient<IAuthenticationService, AuthenticationService>()
                .AddTransient<IRoleService, RoleService>()
                .AddTransient<IAccountService, AccountService>()
                .AddTransient<AuthenticationHeaderHandler>()
                .AddHttpClient(ClientName, client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();
            return builder;
        }
    }
}