using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Contracts.Catalog.Brands;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Wrapper;
using LazyCache;

namespace CleanBlazor.Application.Features.Catalog.Brands.Queries;

[ExcludeFromCodeCoverage]
public sealed record GetAllBrandsQuery : IQuery<Result<List<GetAllBrandsResponse>>>;

internal sealed class GetAllBrandsCachedQueryHandler : IQueryHandler<GetAllBrandsQuery, Result<List<GetAllBrandsResponse>>>
{
    private readonly IAppCache _cache;
    private readonly IBrandRepository _brandRepository;
    private readonly IMapper _mapper;

    public GetAllBrandsCachedQueryHandler(
        IMapper mapper,
        IAppCache cache,
        IBrandRepository brandRepository)
    {
        _mapper = mapper;
        _cache = cache;
        _brandRepository = brandRepository;
    }

    public async Task<Result<List<GetAllBrandsResponse>>> Handle(
        GetAllBrandsQuery request,
        CancellationToken cancellationToken)
    {
        var brands = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllBrandsCacheKey,
                () => _brandRepository.GetAllAsync(cancellationToken));
        var mappedBrands = _mapper.Map<List<GetAllBrandsResponse>>(brands);
        return mappedBrands;
    }
}
