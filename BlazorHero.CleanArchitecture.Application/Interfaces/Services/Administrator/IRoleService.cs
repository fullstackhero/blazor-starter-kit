using BlazorHero.CleanArchitecture.Application.Requests.Roles;
using BlazorHero.CleanArchitecture.Application.Responses.Roles;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Administrator
{
    public interface IRoleService
    {
        Task<Result<GetAllRolesResponse>> GetAllAsync();
        Task<Result<string>> SaveAsync(RoleRequest request);
        Task<Result<string>> DeleteAsync(string id);
    }
}
