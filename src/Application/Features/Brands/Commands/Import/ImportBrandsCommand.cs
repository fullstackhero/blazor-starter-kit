using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using FluentValidation;

namespace BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.Import
{
    public partial class ImportBrandsCommand : IRequest<Result<int>>
    {
        public UploadRequest UploadRequest { get; set; }
    }

    internal class ImportBrandsCommandHandler : IRequestHandler<ImportBrandsCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IExcelService _excelService;
        private readonly IMapper _mapper;
        private readonly IValidator<AddEditBrandCommand> _addBrandValidator;
        private readonly IStringLocalizer<ImportBrandsCommandHandler> _localizer;

        public ImportBrandsCommandHandler(
            IUnitOfWork<int> unitOfWork,
            IExcelService excelService,
            IMapper mapper,
            IValidator<AddEditBrandCommand> addBrandValidator,
            IStringLocalizer<ImportBrandsCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _excelService = excelService;
            _mapper = mapper;
            _addBrandValidator = addBrandValidator;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(ImportBrandsCommand request, CancellationToken cancellationToken)
        {
            var stream = new MemoryStream(request.UploadRequest.Data);
            var result = (await _excelService.ImportAsync(stream, mappers: new Dictionary<string, Func<DataRow, Brand, object>>
            {
                { _localizer["Name"], (row,item) => item.Name = row[_localizer["Name"]].ToString() },
                { _localizer["Description"], (row,item) => item.Description = row[_localizer["Description"]].ToString() },
                { _localizer["Tax"], (row,item) => item.Tax = decimal.TryParse(row[_localizer["Tax"]].ToString(), out var tax) ? tax : 1 }
            }, _localizer["Brands"])).ToList();

            var errors = new List<string>();
            var errorsOccurred = false;
            foreach(var item in result)
            {
                var validationResult = await _addBrandValidator.ValidateAsync(_mapper.Map<AddEditBrandCommand>(item), cancellationToken);
                if (validationResult.IsValid)
                {
                    await _unitOfWork.Repository<Brand>().AddAsync(item);
                }
                else
                {
                    errorsOccurred = true;
                    errors.AddRange(validationResult.Errors.Select(e => $"{(!string.IsNullOrWhiteSpace(item.Name) ? $"{item.Name} - " : string.Empty)}{e.ErrorMessage}"));
                }
            }

            if (errorsOccurred)
            {
                return await Result<int>.FailAsync(errors);
            }

            await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandsCacheKey);
            return await Result<int>.SuccessAsync(result.Count, _localizer["Import Success"]);
        }
    }
}