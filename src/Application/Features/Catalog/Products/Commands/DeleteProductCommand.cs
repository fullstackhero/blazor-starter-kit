using System.Diagnostics.CodeAnalysis;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Wrapper;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.Catalog.Products.Commands;

[ExcludeFromCodeCoverage]
public sealed record DeleteProductCommand(int Id) : ICommand<Result<int>>;

internal sealed class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand, Result<int>>
{
    private readonly IStringLocalizer<DeleteProductCommandHandler> _localizer;
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(
        IUnitOfWork unitOfWork,
        IStringLocalizer<DeleteProductCommandHandler> localizer,
        IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _localizer = localizer;
        _productRepository = productRepository;
    }

    public async Task<Result<int>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product == null)
        {
            return Result.Fail<int>(_localizer["Product Not Found!"]);
        }

        _productRepository.Remove(product);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Ok(product.Id, _localizer["Product Deleted"]);
    }
}
