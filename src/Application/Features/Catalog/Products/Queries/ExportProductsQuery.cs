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

namespace CleanBlazor.Application.Features.Catalog.Products.Queries;

[ExcludeFromCodeCoverage]
public sealed record ExportProductsQuery(string SearchString = "") : IQuery<Result<string>>;

internal sealed class ExportProductsQueryHandler : IQueryHandler<ExportProductsQuery, Result<string>>
{
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<ExportProductsQueryHandler> _localizer;
    private readonly IProductRepository _productRepository;

    public ExportProductsQueryHandler(
        IExcelService excelService,
        IStringLocalizer<ExportProductsQueryHandler> localizer,
        IProductRepository productRepository)
    {
        _excelService = excelService;
        _localizer = localizer;
        _productRepository = productRepository;
    }

    public async Task<Result<string>> Handle(ExportProductsQuery request, CancellationToken cancellationToken)
    {
        ProductFilterSpecification productFilterSpec = new(request.SearchString);
        var products = await _productRepository.Entities
            .Specify(productFilterSpec)
            .ToListAsync(cancellationToken);
        var data = await _excelService.ExportAsync(products,
            new Dictionary<string, Func<Product, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Barcode"], item => item.Barcode },
                { _localizer["Description"], item => item.Description },
                { _localizer["Rate"], item => item.Rate }
            },
            _localizer["Products"]);

        return data;
    }
}
