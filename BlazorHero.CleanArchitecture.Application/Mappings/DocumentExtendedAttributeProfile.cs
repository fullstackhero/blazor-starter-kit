using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.GetById;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;

namespace BlazorHero.CleanArchitecture.Application.Mappings
{
    public class DocumentExtendedAttributeProfile : Profile
    {
        public DocumentExtendedAttributeProfile()
        {
            CreateMap<AddEditDocumentExtendedAttributeCommand, DocumentExtendedAttribute>().ReverseMap();
            CreateMap<GetDocumentExtendedAttributeByIdResponse, DocumentExtendedAttribute>().ReverseMap();
            CreateMap<GetAllDocumentExtendedAttributesResponse, DocumentExtendedAttribute>().ReverseMap();
        }
    }
}