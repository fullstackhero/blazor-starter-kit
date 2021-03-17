using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Repositories
{
    public class BrandRepository : IBrandRepository
    {
        private readonly IRepositoryAsync<Brand> _repository;

        public BrandRepository(IRepositoryAsync<Brand> repository)
        {
            _repository = repository;
        }

        public IQueryable<Brand> Brands => _repository.Entities;

        public async Task DeleteAsync(Brand brand)
        {
            await _repository.DeleteAsync(brand);
        }

        public async Task<Brand> GetByIdAsync(int brandId)
        {
            return await _repository.Entities.Where(p => p.Id == brandId).FirstOrDefaultAsync();
        }

        public async Task<List<Brand>> GetListAsync()
        {
            return await _repository.Entities.ToListAsync();
        }

        public async Task<int> InsertAsync(Brand brand)
        {
            await _repository.AddAsync(brand);
            return brand.Id;
        }

        public async Task UpdateAsync(Brand brand)
        {
            await _repository.UpdateAsync(brand);
        }
    }
}