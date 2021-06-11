using System.Collections.Generic;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.Delete;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;
using BlazorHero.CleanArchitecture.Domain.Entities.Misc;
using BlazorHero.CleanArchitecture.Shared.Wrapper;

namespace BlazorHero.CleanArchitecture.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }

        public static void AddExtendedAttributesHandlers(this IServiceCollection services)
        {
            //TODO - add handlers with reflection!

            #region DocumentExtendedAttribute

            services.AddScoped(typeof(IRequestHandler<AddEditExtendedAttributeCommand<int, int, Document, DocumentExtendedAttribute>, Result<int>>), typeof(AddEditExtendedAttributeCommandHandler<int, int, Document, DocumentExtendedAttribute>));
            services.AddScoped(typeof(IRequestHandler<DeleteExtendedAttributeCommand<int, int, Document, DocumentExtendedAttribute>, Result<int>>), typeof(DeleteExtendedAttributeCommandHandler<int, int, Document, DocumentExtendedAttribute>));
            services.AddScoped(typeof(IRequestHandler<GetAllExtendedAttributesByEntityIdQuery<int, int>, Result<List<GetAllExtendedAttributesByEntityIdResponse<int, int>>>>), typeof(GetAllExtendedAttributesByEntityIdQueryHandler<int, int>));
            services.AddScoped(typeof(IRequestHandler<GetExtendedAttributeByIdQuery<int, int, Document, DocumentExtendedAttribute>, Result<GetExtendedAttributeByIdResponse<int, int>>>), typeof(GetExtendedAttributeByIdQueryHandler<int, int, Document, DocumentExtendedAttribute>));
            services.AddScoped(typeof(IRequestHandler<GetAllExtendedAttributesQuery<int, int, Document, DocumentExtendedAttribute>, Result<List<GetAllExtendedAttributesResponse<int, int>>>>), typeof(GetAllExtendedAttributesQueryHandler<int, int, Document, DocumentExtendedAttribute>));
            services.AddScoped(typeof(IRequestHandler<ExportExtendedAttributesQuery<int, int, Document, DocumentExtendedAttribute>, string>), typeof(ExportExtendedAttributesQueryHandler<int, int, Document, DocumentExtendedAttribute>));

            #endregion DocumentExtendedAttribute
        }
    }
}