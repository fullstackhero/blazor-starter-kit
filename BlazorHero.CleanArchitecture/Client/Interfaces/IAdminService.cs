using BlazorHero.CleanArchitecture.Application.Wrapper;
using BlazorHero.CleanArchitecture.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Interfaces
{
    public interface IAdminService
    {
        Task<List<UserViewModel>> GetUsersAsync();
        Task<List<RoleViewModel>> GetRolesAsync();
    }
}
