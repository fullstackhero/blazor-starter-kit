using CleanBlazor.Domain.Entities.Catalog;

namespace CleanBlazor.Domain.Repositories;

public interface IProductRepository : IRepository<Product, int>
{
    Task<bool> IsBrandUsedAsync(int brandId, CancellationToken cancellationToken = default);
}
