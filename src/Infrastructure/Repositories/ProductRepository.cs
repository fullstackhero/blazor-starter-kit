using CleanBlazor.Domain.Entities.Catalog;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CleanBlazor.Infrastructure.Repositories;

internal sealed class ProductRepository : GenericRepository<Product, int>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsBrandUsedAsync(int brandId, CancellationToken cancellationToken = default) =>
        await Entities.AnyAsync(b => b.BrandId == brandId, cancellationToken);
}
