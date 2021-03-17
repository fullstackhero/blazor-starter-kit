using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Brands.Delete
{
    public class DeleteBrandCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand, Result<int>>
        {
            private readonly IBrandRepository _brandRepository;
            private readonly IProductRepository _productRepository;
            private readonly IUnitOfWork _unitOfWork;

            public DeleteBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork, IProductRepository productRepository)
            {
                _brandRepository = brandRepository;
                _unitOfWork = unitOfWork;
                _productRepository = productRepository;
            }

            public async Task<Result<int>> Handle(DeleteBrandCommand command, CancellationToken cancellationToken)
            {
                var isBrandUsed = await _productRepository.IsBrandUsed(command.Id);
                if (!isBrandUsed)
                {
                    var brand = await _brandRepository.GetByIdAsync(command.Id);
                    await _brandRepository.DeleteAsync(brand);
                    await _unitOfWork.Commit(cancellationToken);
                    return Result<int>.Success(brand.Id, "Brand Deleted!");
                }
                else
                {
                    return Result<int>.Fail("Deletion Not Allowed!");
                }
            }
        }
    }
}