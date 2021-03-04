using BlazorHero.CleanArchitecture.Shared.Requests.Roles;
using BlazorHero.CleanArchitecture.Shared.Responses.Roles;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity
{
    public interface IRoleService
    {
        Task<Result<GetAllRolesResponse>> GetAllAsync();
        Task<Result<string>> SaveAsync(RoleRequest request);
        Task<Result<string>> DeleteAsync(string id);
    }
}
