using System.Diagnostics.CodeAnalysis;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Wrapper;
using LazyCache;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.Catalog.Brands.Commands;

[ExcludeFromCodeCoverage]
public sealed record DeleteBrandCommand(int Id) : ICommand<Result<int>>;

internal sealed class DeleteBrandCommandHandler : ICommandHandler<DeleteBrandCommand, Result<int>>
{
    private readonly IStringLocalizer<DeleteBrandCommandHandler> _localizer;
    private readonly IAppCache _cache;
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBrandCommandHandler(
        IUnitOfWork unitOfWork,
        IProductRepository productRepository,
        IBrandRepository brandRepository,
        IStringLocalizer<DeleteBrandCommandHandler> localizer,
        IAppCache cache)
    {
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _localizer = localizer;
        _cache = cache;
    }

    public async Task<Result<int>> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
    {
        var isBrandUsed = await _productRepository.IsBrandUsedAsync(command.Id, cancellationToken);
        if (isBrandUsed)
        {
            return Result.Fail<int>(_localizer["Deletion Not Allowed"]);
        }

        var brand = await _brandRepository.GetByIdAsync(command.Id, cancellationToken);
        if (brand == null)
        {
            return Result.Fail<int>(_localizer["Brand Not Found!"]);
        }

        _brandRepository.Remove(brand);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cache.Remove(ApplicationConstants.Cache.GetAllBrandsCacheKey);
        return Result.Ok(brand.Id, _localizer["Brand Deleted"]);
    }
}
