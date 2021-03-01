using BlazorHero.CleanArchitecture.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Admin
{
    public partial class Roles
    {
        public List<RoleViewModel> RoleViewModel = new List<RoleViewModel>();
        private RoleViewModel role = new RoleViewModel();
        private string searchString = "";

        protected override async Task OnInitializedAsync()
        {
            GetRolesAsync();
        }

        private List<RoleViewModel> GetRolesAsync()
        {
            RoleViewModel = _adminService.GetRolesAsync().Result;
            return RoleViewModel;
        }

        private async Task SaveAsync()
        {
        }

        private bool Search(RoleViewModel role)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (role.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}