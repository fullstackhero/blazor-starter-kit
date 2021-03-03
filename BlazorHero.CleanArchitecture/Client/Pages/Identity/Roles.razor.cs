using BlazorHero.CleanArchitecture.Application.Requests.Roles;
using BlazorHero.CleanArchitecture.Application.Responses.Roles;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Roles
    {
        public List<RoleResponse> RoleList = new List<RoleResponse>();
        private RoleResponse role = new RoleResponse();
        private string searchString = "";

        protected override async Task OnInitializedAsync()
        {
            await GetRolesAsync();
        }

        private async Task GetRolesAsync()
        {
            var response = await _roleService.GetRolesAsync();
            if(response.Succeeded)
            {
                RoleList =  response.Data.Roles.ToList();
            }
            else
            {
                _snackBar.Add(response.Message, Severity.Error);
            }
        }
        private void Edit(string id)
        {
            role = RoleList.FirstOrDefault(c => c.Id == id);
        }
        private async Task Delete(string id)
        {
            var response = await _roleService.DeleteAsync(id);
            if (response.Succeeded)
            {
                await Reset();
                _snackBar.Add(response.Message, Severity.Success);
            }
            else
            {
                _snackBar.Add(response.Message, Severity.Error);
            }
        }
        private async Task SaveAsync()
        {
            var roleRequest = new RoleRequest() { Name = role.Name, Id = role.Id };
            var response = await _roleService.SaveAsync(roleRequest);
            if(response.Succeeded)
            {
                await Reset();
                _snackBar.Add("Role Saved.", Severity.Success);
            }
            else
            {
                _snackBar.Add(response.Message, Severity.Error);
            }
            
            
        }
        private async Task Reset()
        {
            role = new RoleResponse();
            await GetRolesAsync();
        }
        private bool Search(RoleResponse role)
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