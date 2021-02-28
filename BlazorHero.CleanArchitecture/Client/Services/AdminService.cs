using BlazorHero.CleanArchitecture.Application.Wrapper;
using BlazorHero.CleanArchitecture.Client.Interfaces;
using BlazorHero.CleanArchitecture.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services
{
    public class AdminService : IAdminService
    {
        public async Task<List<RoleViewModel>> GetRolesAsync()
        {
            var roles = new List<RoleViewModel>()
            {
                new RoleViewModel{ Id="1", Name="Admin" },
                 new RoleViewModel{ Id="2", Name="Basic" },
            };
            return roles;
        }

        public async Task<List<UserViewModel>> GetUsersAsync()
        {
            var users = new List<UserViewModel>()
            {
                new UserViewModel{ FirstName = "Mukesh", Id="1" },
                 new UserViewModel{ FirstName = "John", Id="2" },
                  new UserViewModel{ FirstName = "Max", Id="3" },
            };
            return users;

        }
    }
}
