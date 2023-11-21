using AutoMapper;
using CleanBlazor.Application.Features.DocumentManagement.Documents.Commands;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Domain.Entities.Misc;

namespace CleanBlazor.Application.Configuration.Mappings;

public class DocumentProfile : Profile
{
    public DocumentProfile()
    {
        CreateMap<AddEditDocumentCommand, Document>().ReverseMap();
        CreateMap<GetDocumentByIdResponse, Document>().ReverseMap();
    }
}
