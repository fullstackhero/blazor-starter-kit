using CleanBlazor.Domain.Entities.Catalog;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Infrastructure.Data;

namespace CleanBlazor.Infrastructure.Repositories;

internal sealed class BrandRepository : GenericRepository<Brand, int>, IBrandRepository
{
    public BrandRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
}
