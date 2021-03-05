using BlazorHero.CleanArchitecture.Shared.Requests.Roles;
using BlazorHero.CleanArchitecture.Shared.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Identity.Roles
{
    public interface IRoleService : IClientService
    {
        Task<IResult<GetAllRolesResponse>> GetRolesAsync();
        Task<IResult<string>> SaveAsync(RoleRequest role);
        Task<IResult<string>> DeleteAsync(string id);
    }
}
