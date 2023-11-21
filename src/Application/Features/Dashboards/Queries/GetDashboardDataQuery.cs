using System.Diagnostics.CodeAnalysis;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Features.Dashboards.Queries.GetData;
using CleanBlazor.Contracts.Dashboard;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.Dashboards.Queries;

[ExcludeFromCodeCoverage]
public sealed record GetDashboardDataQuery : IQuery<Result<DashboardDataResponse>>;

internal sealed class GetDashboardDataQueryHandler : IQueryHandler<GetDashboardDataQuery, Result<DashboardDataResponse>>
{
    private readonly IStringLocalizer<GetDashboardDataQueryHandler> _localizer;
    private readonly IProductRepository _productRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentTypeRepository _documentTypeRepository;
    private readonly TimeProvider _timeProvider;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;

    public GetDashboardDataQueryHandler(
        IUserService userService,
        IRoleService roleService,
        IStringLocalizer<GetDashboardDataQueryHandler> localizer,
        IProductRepository productRepository,
        IBrandRepository brandRepository,
        IDocumentRepository documentRepository,
        IDocumentTypeRepository documentTypeRepository,
        TimeProvider timeProvider)
    {
        _userService = userService;
        _roleService = roleService;
        _localizer = localizer;
        _productRepository = productRepository;
        _brandRepository = brandRepository;
        _documentRepository = documentRepository;
        _documentTypeRepository = documentTypeRepository;
        _timeProvider = timeProvider;
    }

    public async Task<Result<DashboardDataResponse>> Handle(
        GetDashboardDataQuery query,
        CancellationToken cancellationToken)
    {
        DashboardDataResponse response = new()
        {
            ProductCount = await _productRepository.Entities.CountAsync(cancellationToken),
            BrandCount = await _brandRepository.Entities.CountAsync(cancellationToken),
            DocumentCount = await _documentRepository.Entities.CountAsync(cancellationToken),
            DocumentTypeCount = await _documentTypeRepository.Entities.CountAsync(cancellationToken),
            UserCount = await _userService.GetCountAsync(),
            RoleCount = await _roleService.GetCountAsync()
        };

        var selectedYear = _timeProvider.GetUtcNow().Year;
        var productsFigure = new double[13];
        var brandsFigure = new double[13];
        var documentsFigure = new double[13];
        var documentTypesFigure = new double[13];
        for (var month = 1; month <= 12; month++)
        {
            DateTimeOffset filterStartDate = new(selectedYear, month, 01, 0, 0, 0, TimeSpan.Zero);
            DateTimeOffset filterEndDate =
                new(selectedYear,
                    month,
                    DateTime.DaysInMonth(selectedYear, month),
                    23,
                    59,
                    59,
                    TimeSpan.Zero); // Monthly Based

            productsFigure[month - 1] = await _productRepository.Entities
                .Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate)
                .CountAsync(cancellationToken);
            brandsFigure[month - 1] = await _brandRepository.Entities
                .Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate)
                .CountAsync(cancellationToken);
            documentsFigure[month - 1] = await _documentRepository.Entities
                .Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate)
                .CountAsync(cancellationToken);
            documentTypesFigure[month - 1] = await _documentTypeRepository.Entities
                .Where(x => x.CreatedOn >= filterStartDate && x.CreatedOn <= filterEndDate)
                .CountAsync(cancellationToken);
        }

        response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Products"], Data = productsFigure });
        response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Brands"], Data = brandsFigure });
        response.DataEnterBarChart.Add(new ChartSeries { Name = _localizer["Documents"], Data = documentsFigure });
        response.DataEnterBarChart.Add(new ChartSeries
        {
            Name = _localizer["Document Types"], Data = documentTypesFigure
        });

        return response;
    }
}
