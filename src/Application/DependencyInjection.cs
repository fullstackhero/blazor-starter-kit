using System.Reflection;
using CleanBlazor.Application.Abstractions.Serialization;
using CleanBlazor.Application.Behaviors;
using CleanBlazor.Application.Configuration;
using CleanBlazor.Application.Serialization;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanBlazor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        var executingAssembly = Assembly.GetExecutingAssembly();

        services
            .AddAutoMapper(executingAssembly)
            .AddMediatR(config => config.RegisterServicesFromAssembly(executingAssembly))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>))
            .AddScoped<IJsonSerializer, SystemTextJsonSerializer>(); // you can change it

        services.AddOptions<AppConfiguration>().Bind(configuration.GetSection(nameof(AppConfiguration)));
        services.AddOptions<MailConfiguration>().Bind(configuration.GetSection(nameof(MailConfiguration)));

        return services;
    }
}
