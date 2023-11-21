using AutoMapper;
using CleanBlazor.Application.Features.DocumentManagement.DocumentTypes.Commands;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Domain.Entities.Misc;

namespace CleanBlazor.Application.Configuration.Mappings;

public class DocumentTypeProfile : Profile
{
    public DocumentTypeProfile()
    {
        CreateMap<AddEditDocumentTypeCommand, DocumentType>().ReverseMap();
        CreateMap<GetDocumentTypeByIdResponse, DocumentType>().ReverseMap();
        CreateMap<GetAllDocumentTypesResponse, DocumentType>().ReverseMap();
    }
}
