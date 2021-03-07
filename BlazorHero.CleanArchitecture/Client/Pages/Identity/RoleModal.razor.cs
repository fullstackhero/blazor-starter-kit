using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class RoleModal
    {
        bool success;
        string[] errors = { };
        MudForm form;
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        [Required]
        public string Name { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task SaveAsync()
        {
            form.Validate();
            if(form.IsValid)
            {
                var roleRequest = new RoleRequest() { Name = Name, Id = Id };
                var response = await _roleManager.SaveAsync(roleRequest);
                if(response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
                MudDialog.Close();
            }
            
        }
    }
}
