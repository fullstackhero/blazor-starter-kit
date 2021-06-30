using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.ImportExcel
{
    public partial class ImportBrandCommand:IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }
    internal class ImportBrandCommandHandler : IRequestHandler<ImportBrandCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> unitOfWork;
        private readonly IMapper mapper;
        private readonly IExcelService excelService;
        private readonly IStringLocalizer<ImportBrandCommandHandler> _localizer;

        public ImportBrandCommandHandler(IUnitOfWork<int> unitOfWork, 
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ImportBrandCommandHandler> localizer)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.excelService = excelService;
            this._localizer = localizer;
        }
        public async Task<Result<int>> Handle(ImportBrandCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result =await excelService.ImportAsync<Brand>(stream, mappers: new Dictionary<string, Func<Brand,DataRow, object>>
            {
                { _localizer["Name"], (item,row)=> item.Name =row[_localizer["Name"]].ToString()},
                { _localizer["Description"], (item,row)=> item.Description =row[_localizer["Description"]].ToString() },
                { _localizer["Tax"], (item,row)=> item.Tax =Convert.ToDecimal(row[_localizer["Tax"]]??0) }
            },"Brands");
            foreach(var item in result)
            {
                await this.unitOfWork.Repository<Brand>().AddAsync(item);
            }
            await this.unitOfWork.CommitAndRemoveCache(cancellationToken);
            return await  Result<int>.SuccessAsync(_localizer["Import Success"]);
        }
    }
}
