using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class UserRoles
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Description { get; set; }

        public List<UserRoleModel> UserRolesList { get; set; } = new List<UserRoleModel>();
        public ClaimsPrincipal CurrentUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentUser = await _authenticationManager.CurrentUser();

            var userId = Id;
            var result = await _userManager.GetAsync(userId);
            if (result.Succeeded)
            {
                var user = result.Data;
                if (user != null)
                {
                    Title = $"{user.FirstName} {user.LastName}";
                    Description = $"Manage {user.FirstName} {user.LastName}'s Roles";
                    var response = await _userManager.GetRolesAsync(user.Id);
                    UserRolesList = response.Data.UserRoles;
                }
            }
        }
        private async Task SaveAsync()
        {
            var request = new UpdateUserRolesRequest()
            {
                UserId = Id,
                UserRoles = UserRolesList
            };
            var result = await _userManager.UpdateRolesAsync(request);
            if (result.Succeeded)
            {
                _snackBar.Add(localizer[result.Messages[0]], Severity.Success);
                _navigationManager.NavigateTo("/identity/users");
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    _snackBar.Add(localizer[error], Severity.Error);
                }
            }
        }
    }
}