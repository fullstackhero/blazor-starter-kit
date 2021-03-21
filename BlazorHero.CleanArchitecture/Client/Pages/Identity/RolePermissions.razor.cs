using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Mappings;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class RolePermissions
    {
        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Description { get; set; }

        public PermissionResponse model { get; set; }
        private IMapper _mapper;

        protected override async Task OnInitializedAsync()
        {
            _mapper = new MapperConfiguration(c => { c.AddProfile<RoleProfile>(); }).CreateMapper();
            var roleId = Id;
            var result = await _roleManager.GetPermissionsAsync(roleId);
            if (result.Succeeded)
            {
                model = result.Data;
                if (model != null)
                {
                    Description = $"{localizer["Manage"]} {model.RoleId} {model.RoleName}'s {localizer["Permissions"]}";
                }
            }
        }
        private async Task SaveAsync()
        {
            var request = _mapper.Map<PermissionResponse, PermissionRequest>(model);
            var result = await _roleManager.UpdatePermissionsAsync(request);
            if (result.Succeeded)
            {
                _snackBar.Add(localizer[result.Messages[0]], Severity.Success);
                _navigationManager.NavigateTo("/identity/roles");
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