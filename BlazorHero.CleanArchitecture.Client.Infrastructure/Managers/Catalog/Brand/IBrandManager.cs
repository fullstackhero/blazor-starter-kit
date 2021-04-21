using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Queries.GetAll;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Brand
{
    public interface IBrandManager : IManager
    {
        Task<IResult<List<GetAllBrandsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditBrandCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}