using System.Diagnostics.CodeAnalysis;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Extensions;
using CleanBlazor.Application.Specifications.Catalog;
using CleanBlazor.Domain.Entities.Catalog;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.Catalog.Brands.Queries;

[ExcludeFromCodeCoverage]
public sealed record ExportBrandsQuery(string SearchString = "") : IQuery<Result<string>>;

internal sealed class ExportBrandsQueryHandler : IQueryHandler<ExportBrandsQuery, Result<string>>
{
    private readonly IBrandRepository _brandRepository;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportBrandsQueryHandler> _localizer;

    public ExportBrandsQueryHandler(
        IExcelService excelService,
        IStringLocalizer<ExportBrandsQueryHandler> localizer,
        IBrandRepository brandRepository)
    {
        _excelService = excelService;
        _localizer = localizer;
        _brandRepository = brandRepository;
    }

    public async Task<Result<string>> Handle(ExportBrandsQuery request, CancellationToken cancellationToken)
    {
        BrandFilterSpecification brandFilterSpec = new(request.SearchString);
        var brands = await _brandRepository.Entities
            .Specify(brandFilterSpec)
            .ToListAsync(cancellationToken);
        var data = await _excelService.ExportAsync(brands,
            new Dictionary<string, Func<Brand, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["Tax"], item => item.Tax }
            },
            _localizer["Brands"]);

        return data;
    }
}
