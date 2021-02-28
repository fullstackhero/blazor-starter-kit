using BlazorHero.CleanArchitecture.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Admin
{
    public partial class Users
    {
        public List<UserViewModel> UsersViewModel = new List<UserViewModel>();
        private UserViewModel user = new UserViewModel();
        private string searchString = "";
        protected override async Task OnInitializedAsync()
        {
            GetUsersAsync();
        }
        private List<UserViewModel> GetUsersAsync()
        {
            UsersViewModel =  _adminService.GetUsersAsync().Result;
            return UsersViewModel;
        }
        private bool Search(UserViewModel user)
        {
            if (string.IsNullOrWhiteSpace(searchString)) return true;
            if (user.FirstName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
    }
}
