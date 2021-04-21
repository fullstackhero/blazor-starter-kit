using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.DataAccess.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task<bool> IsBrandUsed(int brandId);
    }
}