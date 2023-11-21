using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Wrapper;

namespace CleanBlazor.Application.Features.DocumentManagement.DocumentTypes.Queries;

[ExcludeFromCodeCoverage]
public sealed record GetDocumentTypeByIdQuery(int Id) : IQuery<Result<GetDocumentTypeByIdResponse>>;

internal sealed class GetDocumentTypeByIdQueryHandler
    : IQueryHandler<GetDocumentTypeByIdQuery, Result<GetDocumentTypeByIdResponse>>
{
    private readonly IDocumentTypeRepository _documentTypeRepository;
    private readonly IMapper _mapper;

    public GetDocumentTypeByIdQueryHandler(IDocumentTypeRepository documentTypeRepository, IMapper mapper)
    {
        _documentTypeRepository = documentTypeRepository;
        _mapper = mapper;
    }

    public async Task<Result<GetDocumentTypeByIdResponse>> Handle(
        GetDocumentTypeByIdQuery query,
        CancellationToken cancellationToken)
    {
        var documentType = await _documentTypeRepository.GetByIdAsync(query.Id, cancellationToken);
        var mappedDocumentType = _mapper.Map<GetDocumentTypeByIdResponse>(documentType);
        return mappedDocumentType;
    }
}
