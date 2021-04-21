using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorHero.CleanArchitecture.RoleService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRoleServiceMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}