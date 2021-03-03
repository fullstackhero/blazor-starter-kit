using BlazorHero.CleanArchitecture.Application.Requests.Roles;
using BlazorHero.CleanArchitecture.Application.Responses.Roles;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using BlazorHero.CleanArchitecture.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services.Roles
{
    public interface IRoleService
    {
        Task<Result<GetAllRolesResponse>> GetRolesAsync();
        Task<IResult<string>> SaveAsync(RoleRequest role);
        Task<IResult<string>> DeleteAsync(string id);
    }
}
