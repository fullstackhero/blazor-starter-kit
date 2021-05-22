using BlazorHero.CleanArchitecture.Application.Configurations;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorHero.CleanArchitecture.Server.Extensions
{
    public static class MvcBuilderExtensions
    {
        public static IMvcBuilder AddValidators(this IMvcBuilder builder)
        {
            builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AppConfiguration>());
            return builder;
        }
    }
}