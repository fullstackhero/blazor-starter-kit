using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
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

namespace BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.Import
{
    public partial class ImportBrandsCommand:IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }
    internal class ImportBrandsCommandHandler : IRequestHandler<ImportBrandsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> unitOfWork;
        private readonly IMapper mapper;
        private readonly IExcelService excelService;
        private readonly IStringLocalizer<ImportBrandsCommandHandler> _localizer;

        public ImportBrandsCommandHandler(IUnitOfWork<int> unitOfWork, 
            IMapper mapper,
            IExcelService excelService,
            IStringLocalizer<ImportBrandsCommandHandler> localizer)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.excelService = excelService;
            this._localizer = localizer;
        }
        public async Task<Result<int>> Handle(ImportBrandsCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result =await excelService.ImportAsync<Brand>(stream, mappers: new Dictionary<string, Func<DataRow, Brand, object>>
            {
                { _localizer["Name"], (row,item)=> item.Name =row[_localizer["Name"]].ToString()},
                { _localizer["Description"], (row,item)=> item.Description =row[_localizer["Description"]].ToString() },
                { _localizer["Tax"], (row,item)=> item.Tax =Convert.ToDecimal(row[_localizer["Tax"]]??0) }
            },"Brands");
            foreach(var item in result)
            {
                await this.unitOfWork.Repository<Brand>().AddAsync(item);
            }
            await this.unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandsCacheKey);
            return await  Result<int>.SuccessAsync(_localizer["Import Success"]);
        }
    }
}
