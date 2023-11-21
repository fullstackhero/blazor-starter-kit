using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Domain.Entities.Misc;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Wrapper;

namespace CleanBlazor.Application.Features.DocumentManagement.Documents.Queries;

[ExcludeFromCodeCoverage]
public sealed record GetDocumentByIdQuery(int Id) : IQuery<Result<GetDocumentByIdResponse>>;

internal sealed class GetDocumentByIdQueryHandler : IQueryHandler<GetDocumentByIdQuery, Result<GetDocumentByIdResponse>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IMapper _mapper;

    public GetDocumentByIdQueryHandler(IDocumentRepository documentRepository, IMapper mapper)
    {
        _documentRepository = documentRepository;
        _mapper = mapper;
    }

    public async Task<Result<GetDocumentByIdResponse>> Handle(
        GetDocumentByIdQuery query,
        CancellationToken cancellationToken)
    {
        Document document = await _documentRepository.GetByIdAsync(query.Id, cancellationToken);
        var mappedDocument = _mapper.Map<GetDocumentByIdResponse>(document);
        return mappedDocument;
    }
}
