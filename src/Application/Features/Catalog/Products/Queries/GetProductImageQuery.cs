using System.Diagnostics.CodeAnalysis;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace CleanBlazor.Application.Features.Catalog.Products.Queries;

[ExcludeFromCodeCoverage]
public sealed record GetProductImageQuery(int Id) : IQuery<Result<string>>;

internal sealed class GetProductImageQueryHandler : IQueryHandler<GetProductImageQuery, Result<string>>
{
    private readonly IProductRepository _productRepository;

    public GetProductImageQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<string>> Handle(GetProductImageQuery request, CancellationToken cancellationToken)
    {
        var data = await _productRepository.Entities
            .Where(p => p.Id == request.Id)
            .Select(a => a.ImageDataUrl)
            .FirstOrDefaultAsync(cancellationToken);

        return data;
    }
}
