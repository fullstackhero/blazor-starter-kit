using AutoMapper;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.DataAccess.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.UploadService.Interfaces;
using BlazorHero.CleanArchitecture.UploadService.Interfaces.Requests;
using BlazorHero.CleanArchitecture.Utils.Wrapper;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Products.Commands.AddEdit
{
    public partial class AddEditProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string ImageDataURL { get; set; }
        public decimal Rate { get; set; }
        public int BrandId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    public class AddEditProductCommandHandler : IRequestHandler<AddEditProductCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductCommandHandler> _localizer;

        public AddEditProductCommandHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IUploadService uploadService,
            IStringLocalizer<AddEditProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductCommand command, CancellationToken cancellationToken)
        {
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