using System.Data;
using System.Diagnostics.CodeAnalysis;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Contracts;
using CleanBlazor.Domain.Entities.Catalog;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Wrapper;
using FluentValidation.Results;
using LazyCache;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.Catalog.Brands.Commands;

[ExcludeFromCodeCoverage]
public sealed record ImportBrandsCommand(UploadRequest UploadRequest) : ICommand<Result<int>>;

internal sealed class ImportBrandsCommandHandler : ICommandHandler<ImportBrandsCommand, Result<int>>
{
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ImportBrandsCommandHandler> _localizer;
    private readonly IBrandRepository _brandRepository;
    private readonly IAppCache _cache;
    private readonly IUnitOfWork _unitOfWork;

    public ImportBrandsCommandHandler(
        IUnitOfWork unitOfWork,
        IExcelService excelService,
        IStringLocalizer<ImportBrandsCommandHandler> localizer,
        IBrandRepository brandRepository,
        IAppCache cache)
    {
        _unitOfWork = unitOfWork;
        _excelService = excelService;
        _localizer = localizer;
        _brandRepository = brandRepository;
        _cache = cache;
    }

    public async Task<Result<int>> Handle(ImportBrandsCommand request, CancellationToken cancellationToken)
    {
        MemoryStream stream = new(request.UploadRequest.Data);
        Result<IEnumerable<Brand>> result = await _excelService.ImportAsync(stream,
            new Dictionary<string, Func<DataRow, Brand, object>>
            {
                { _localizer["Name"], (row, item) => item.Name = row[_localizer["Name"]].ToString() },
                {
                    _localizer["Description"],
                    (row, item) => item.Description = row[_localizer["Description"]].ToString()
                },
                {
                    _localizer["Tax"], (row, item) =>
                        item.Tax = decimal.TryParse(row[_localizer["Tax"]].ToString(), out var tax) ? tax : 1
                }
            },
            _localizer["Brands"]);

        if (result.IsFailure)
        {
            return Result.Fail<int>(result.ErrorMessages);
        }

        IEnumerable<Brand> importedBrands = result.Data;
        List<string> errors = new();
        var errorsOccurred = false;
        foreach (Brand brand in importedBrands)
        {
            ValidationResult validationResult = ValidateBrand(brand);
            if (validationResult.IsValid)
            {
                _brandRepository.Add(brand);
            }
            else
            {
                errorsOccurred = true;
                errors.AddRange(validationResult.Errors.Select(e =>
                    $"{(!string.IsNullOrWhiteSpace(brand.Name) ? $"{brand.Name} - " : string.Empty)}{e.ErrorMessage}"));
            }
        }

        if (errorsOccurred)
        {
            return Result.Fail<int>(errors);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cache.Remove(ApplicationConstants.Cache.GetAllBrandsCacheKey);
        return Result.Ok(result.Data.Count(), result.SuccessMessage);
    }

    private ValidationResult ValidateBrand(Brand brand)
    {
        var errors = new List<ValidationFailure>();

        if (string.IsNullOrWhiteSpace(brand.Name))
        {
            errors.Add(new ValidationFailure("Name", _localizer["Name is required!"]));
        }

        if (string.IsNullOrWhiteSpace(brand.Description))
        {
            errors.Add(new ValidationFailure("Description", _localizer["Description is required!"]));
        }

        if (brand.Tax <= 0)
        {
            errors.Add(new ValidationFailure("Tax", _localizer["Tax must be greater than 0"]));
        }

        return new ValidationResult(errors);
    }
}
