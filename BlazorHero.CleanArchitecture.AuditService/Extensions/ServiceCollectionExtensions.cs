using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BlazorHero.CleanArchitecture.AuditService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAuditServiceMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}