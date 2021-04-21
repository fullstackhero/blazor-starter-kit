using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorHero.CleanArchitecture.ChatService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddChatServiceMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}