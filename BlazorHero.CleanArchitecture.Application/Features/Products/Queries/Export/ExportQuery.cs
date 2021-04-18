using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Products.Queries.Export
{
    public class ExportQuery : IRequest<string>
    {
    }

    public class ExportQueryHandler : IRequestHandler<ExportQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<ExportQueryHandler> _localizer;

        public ExportQueryHandler(IExcelService excelService
            , IUnitOfWork unitOfWork
            , IStringLocalizer<ExportQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<string> Handle(ExportQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            var data = await _excelService.ExportAsync(products, mappers: new Dictionary<string, Func<Product, object>>()
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Barcode"], item => item.Barcode },
                { _localizer["Description"], item => item.Description },
                { _localizer["Rate"], item => item.Rate }
            }, sheetName: _localizer["Products"]);

            return data;
        }
    }
}