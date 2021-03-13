using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class AddEditProductModal
    {
        bool success;
        string[] errors = { };
        MudForm form;
        [Parameter]
        public int Id { get; set; }
        [Parameter]
        [Required]
        public string Name { get; set; }
        [Parameter]
        [Required]
        public string Barcode { get; set; }
        [Parameter]
        [Required]
        public string Description { get; set; }
        [Parameter]
        [Required]
        public decimal Rate { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task SaveAsync()
        {
            form.Validate();
            if (form.IsValid)
            {
                //var roleRequest = new RoleRequest() { Name = Name, Id = Id };
                //var response = await _roleManager.SaveAsync(roleRequest);
                //if (response.Succeeded)
                //{
                //    _snackBar.Add(response.Messages[0], Severity.Success);
                //    MudDialog.Close();
                //}
                //else
                //{
                //    foreach (var message in response.Messages)
                //    {
                //        _snackBar.Add(message, Severity.Error);
                //    }
                //}
            }

        }
    }
}
