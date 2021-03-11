using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Generic;
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

        public IQueryable<Product> Products => _repository.Entities;

        public async Task DeleteAsync(Product product)
        {
            await _repository.DeleteAsync(product);
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            return await _repository.Entities.Where(p => p.Id == productId && p.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetListAsync()
        {
            return await _repository.Entities.Where(p=> p.IsDeleted == false).ToListAsync();
        }

        public async Task<int> InsertAsync(Product product)
        {
            await _repository.AddAsync(product);
            return product.Id;
        }

        public async Task UpdateAsync(Product product)
        {
            await _repository.UpdateAsync(product);
        }
    }
}