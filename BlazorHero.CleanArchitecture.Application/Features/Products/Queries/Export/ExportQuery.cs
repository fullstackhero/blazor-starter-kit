using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Products.Queries.Export
{
    public class ExportQuery : IRequest<string>
    {
    }

    public class ExportQueryHandler : IRequestHandler<ExportQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork _unitOfWork;

        public ExportQueryHandler(IExcelService excelService
            , IUnitOfWork unitOfWork)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(ExportQuery request, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.Repository<Product>().GetAllAsync();
            var data = await _excelService.ExportAsync(products, mappers: new Dictionary<string, Func<Product, object>>()
            {
                { "Id", item => item.Id },
                { "Name", item => item.Name },
                { "Barcode", item => item.Barcode },
                { "Description", item => item.Description },
                { "Rate", item => item.Rate }
            }, sheetName: "Products");

            return data;
        }
    }
}