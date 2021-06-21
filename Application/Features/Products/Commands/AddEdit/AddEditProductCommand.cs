using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Requests;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlazorHero.CleanArchitecture.Application.Features.Products.Commands.AddEdit
{
    public partial class AddEditProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageDataURL { get; set; }
        [Required]
        public decimal Rate { get; set; }
        [Required]
        public int BrandId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditProductCommandHandler : IRequestHandler<AddEditProductCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCommandHandler> _localizer;

        public AddEditProductCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Product>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Barcode == command.Barcode, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Barcode already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.Barcode}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var product = _mapper.Map<Product>(command);
                if (uploadRequest != null)
                {
                    product.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                await _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(product.Id, _localizer["Product Saved"]);
            }
            else
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id);
                if (product != null)
                {
                    product.Name = command.Name ?? product.Name;
                    product.Description = command.Description ?? product.Description;
                    if (uploadRequest != null)
                    {
                        product.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }
                    product.Rate = (command.Rate == 0) ? product.Rate : command.Rate;
                    product.BrandId = (command.BrandId == 0) ? product.BrandId : command.BrandId;
                    await _unitOfWork.Repository<Product>().UpdateAsync(product);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(product.Id, _localizer["Product Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Product Not Found!"]);
                }
            }
        }
    }
}