using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Authentication;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Preferences;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace BlazorHero.CleanArchitecture.Client.InServerMode.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ClientName = "BlazorHero.API";

        public static IServiceCollection AddClientServices(this IServiceCollection services,  IConfiguration configuration)
        {
            services
                .AddLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                })
                .AddAuthorizationCore(options =>
                {
                    RegisterPermissionClaims(options);
                })
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
                .AddScoped<ClientPreferenceManager>()
                .AddScoped<BlazorHeroStateProvider>()
                .AddScoped<AuthenticationStateProvider, BlazorHeroStateProvider>()
                .AddManagers()
                .AddTransient<AuthenticationHeaderHandler>()
                .AddScoped(sp => sp
                    .GetRequiredService<IHttpClientFactory>()
                    .CreateClient(ClientName).EnableIntercept(sp))
                .AddHttpClient(ClientName, client =>
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Clear();
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
                    client.BaseAddress = new Uri(configuration.GetValue<string>("ServerURI"));
                })
                .AddHttpMessageHandler<AuthenticationHeaderHandler>();
            services.AddHttpClientInterceptor();
            services.AddSignalR();

            return services;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            var managers = typeof(IManager);

            var types = managers
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
            {
                if (managers.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
            }

            return services;
        }

        private static void RegisterPermissionClaims(AuthorizationOptions options)
        {
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
                }
            }
        }
    }
}