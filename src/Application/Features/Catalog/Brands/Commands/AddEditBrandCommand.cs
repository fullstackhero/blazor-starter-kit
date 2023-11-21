using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Domain.Entities.Catalog;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Wrapper;
using LazyCache;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.Catalog.Brands.Commands;

[ExcludeFromCodeCoverage]
public sealed record AddEditBrandCommand(int Id, string Name, string Description, decimal Tax) : ICommand<Result<int>>;

internal sealed class AddEditBrandCommandHandler : ICommandHandler<AddEditBrandCommand, Result<int>>
{
    private readonly IStringLocalizer<AddEditBrandCommandHandler> _localizer;
    private readonly IBrandRepository _brandRepository;
    private readonly IAppCache _cache;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddEditBrandCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizer<AddEditBrandCommandHandler> localizer,
        IBrandRepository brandRepository,
        IAppCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
        _brandRepository = brandRepository;
        _cache = cache;
    }

    public async Task<Result<int>> Handle(AddEditBrandCommand command, CancellationToken cancellationToken)
    {
        if (command.Id == 0)
        {
            var brand = _mapper.Map<Brand>(command);
            _brandRepository.Add(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove(ApplicationConstants.Cache.GetAllBrandsCacheKey);
            return Result.Ok(brand.Id, _localizer["Brand Saved"]);
        }
        else
        {
            var brand = await _brandRepository.GetByIdAsync(command.Id, cancellationToken);
            if (brand == null)
            {
                return Result.Fail<int>(_localizer["Brand Not Found!"]);
            }

            brand.Name = command.Name ?? brand.Name;
            brand.Tax = command.Tax == 0 ? brand.Tax : command.Tax;
            brand.Description = command.Description ?? brand.Description;

            _brandRepository.Update(brand);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove(ApplicationConstants.Cache.GetAllBrandsCacheKey);
            return Result.Ok(brand.Id, _localizer["Brand Updated"]);
        }
    }
}
