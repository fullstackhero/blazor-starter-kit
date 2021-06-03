using System;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Storage;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Storage.Provider;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Storage.Serialization;
using BlazorHero.CleanArchitecture.Infrastructure.Services.Storage;
using BlazorHero.CleanArchitecture.Infrastructure.Services.Storage.JsonConverters;
using BlazorHero.CleanArchitecture.Infrastructure.Services.Storage.Provider;
using BlazorHero.CleanArchitecture.Infrastructure.Services.Storage.Serialization;
using BlazorHero.CleanArchitecture.Infrastructure.Services.Storage.StorageOptions;

namespace BlazorHero.CleanArchitecture.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddInfrastructureMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        public static IServiceCollection AddServerStorage(this IServiceCollection services)
            => AddServerStorage(services, null);

        public static IServiceCollection AddServerStorage(this IServiceCollection services, Action<ServerStorageOptions> configure)
        {
            return services
                .AddScoped<IJsonSerializer, SystemTextJsonSerializer>()
                .AddScoped<IStorageProvider, ServerStorageProvider>()
                .AddScoped<IServerStorageService, ServerStorageService>()
                .AddScoped<ISyncServerStorageService, ServerStorageService>()
                .Configure<ServerStorageOptions>(configureOptions =>
                {
                    configure?.Invoke(configureOptions);
                    configureOptions.JsonSerializerOptions.Converters.Add(new TimespanJsonConverter());
                });
        }
    }
}