using BlazorHero.CleanArchitecture.Application.Configurations;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;
using BlazorHero.CleanArchitecture.Domain.Entities.Misc;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorHero.CleanArchitecture.Server.Extensions
{
    public static class MvcBuilderExtensions
    {
        internal static IMvcBuilder AddValidators(this IMvcBuilder builder, IServiceCollection services)
        {
            builder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AppConfiguration>());
            services.AddExtendedAttributesValidators();
            return builder;
        }

        private static void AddExtendedAttributesValidators(this IServiceCollection services)
        {
            //TODO - add more
            //services.AddScoped(typeof(IValidator<AddEditExtendedAttributeCommand<int, int, Document, DocumentExtendedAttribute>>), typeof(AddEditExtendedAttributeCommandValidator<int, int, Document, DocumentExtendedAttribute>));
            //services.AddScoped(typeof(AbstractValidator<AddEditExtendedAttributeCommand<int, int, Document, DocumentExtendedAttribute>>), typeof(AddEditExtendedAttributeCommandValidator<int, int, Document, DocumentExtendedAttribute>));
        }
    }
}