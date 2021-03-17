using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Features.Brands.AddEdit
{
    public partial class AddEditBrandCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Tax { get; set; }
    }

    public class AddEditBrandCommandHandler : IRequestHandler<AddEditBrandCommand, Result<int>>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public AddEditBrandCommandHandler(IBrandRepository brandRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(AddEditBrandCommand command, CancellationToken cancellationToken)
        {
            var brand = _mapper.Map<Brand>(command);
            if (brand.Id == 0)
            {
                await _brandRepository.InsertAsync(brand);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(brand.Id, "Brand Saved!");
            }
            else
            {
                await _brandRepository.UpdateAsync(brand);
                await _unitOfWork.Commit(cancellationToken);
                return Result<int>.Success(brand.Id, "Brand Updated!");
            }
        }
    }
}