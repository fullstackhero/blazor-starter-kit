using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IRepositoryAsync<Product> _repository;

        public ProductRepository(IRepositoryAsync<Product> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsBrandUsed(int brandId)
        {
            return await _repository.Entities.AnyAsync(b => b.BrandId == brandId);
        }
    }
}