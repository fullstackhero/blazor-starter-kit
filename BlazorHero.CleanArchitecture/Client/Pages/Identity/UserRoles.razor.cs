using System;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class UserRoles
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }
        public List<UserRoleModel> UserRolesList { get; set; } = new();

        private UserRoleModel _userRole = new();
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateUserRoles;
        private bool _canEditUserRoles;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateUserRoles = _authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Create).Result.Succeeded;
            _canEditUserRoles = _authorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.Edit).Result.Succeeded;

            var userId = Id;
            var result = await _userManager.GetAsync(userId);
            if (result.Succeeded)
            {
                var user = result.Data;
                if (user != null)
                {
                    Title = $"{user.FirstName} {user.LastName}";
                    Description = string.Format(_localizer["Manage {0} {1}'s Roles"], user.FirstName, user.LastName);
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
                _snackBar.Add(result.Messages[0], Severity.Success);
                _navigationManager.NavigateTo("/identity/users");
            }
            else
            {
                foreach (var error in result.Messages)
                {
                    _snackBar.Add(error, Severity.Error);
                }
            }
        }

        private bool Search(UserRoleModel userRole)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (userRole.RoleName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (userRole.RoleDescription?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }
    }
}