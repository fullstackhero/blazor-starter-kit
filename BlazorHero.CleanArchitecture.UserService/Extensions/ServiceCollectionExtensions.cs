using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorHero.CleanArchitecture.UserService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUserServiceMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}