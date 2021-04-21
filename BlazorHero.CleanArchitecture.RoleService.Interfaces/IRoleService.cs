using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.RoleService.Interfaces.Requests;
using BlazorHero.CleanArchitecture.RoleService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Utils;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.RoleService.Interfaces
{
    public interface IRoleService : IService
    {
        Task<Result<List<RoleResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleResponse>> GetByIdAsync(string id);

        Task<Result<string>> SaveAsync(RoleRequest request);

        Task<Result<string>> DeleteAsync(string id);

        Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId);

        Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request);
    }
}