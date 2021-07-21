using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BlazorHero.CleanArchitecture.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }
  public class DeleteCheckedProductCommand : IRequest<Result<int[]>>
  {
    public int[] Id  { get; set; }
  }
  internal class DeleteProductCommandHandler :
    IRequestHandler<DeleteCheckedProductCommand, Result<int[]>>,
    IRequestHandler<DeleteProductCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductCommandHandler> _localizer;

        public DeleteProductCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(command.Id);
            if (product != null)
            {
                await _unitOfWork.Repository<Product>().DeleteAsync(product);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(product.Id, _localizer["Product Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Product Not Found!"]);
            }
        }

    public async Task<Result<int[]>> Handle(DeleteCheckedProductCommand request, CancellationToken cancellationToken)
    {
      var products = await _unitOfWork.Repository<Product>().Entities.Where(x=>request.Id.Contains(x.Id)).ToListAsync();
      foreach(var product in products)
      {
        await _unitOfWork.Repository<Product>().DeleteAsync(product);
      }
      await _unitOfWork.Commit(cancellationToken);
      return await Result<int[]>.SuccessAsync(request.Id, string.Format(_localizer["{0} Products Deleted"],request.Id.Length));
    }
  }
}