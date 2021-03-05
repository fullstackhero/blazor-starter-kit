using BlazorHero.CleanArchitecture.Shared.Responses.Identity;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Users
    {
        public List<UserResponse> UserList = new List<UserResponse>();
        private UserResponse user = new UserResponse();
        private string searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await GetUsersAsync();
        }
        private async Task GetUsersAsync()
        {
            var response = await _userService.GetAllAsync();
            if (response.Succeeded)
            {
                UserList = response.Data.Users.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }

            }
        }

        private bool Search(UserResponse user)
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