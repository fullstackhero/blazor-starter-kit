using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

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
    }

    public class AddEditProductCommandHandler : IRequestHandler<AddEditProductCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AddEditProductCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(AddEditProductCommand command, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(command);
            if (product.Id == 0)
            {
                await _unitOfWork.Repository<Product>().AddAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(product.Id, "Product Saved");
            }
            else
            {
                await _unitOfWork.Repository<Product>().UpdateAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(product.Id, "Product Updated");
            }
        }
    }
}