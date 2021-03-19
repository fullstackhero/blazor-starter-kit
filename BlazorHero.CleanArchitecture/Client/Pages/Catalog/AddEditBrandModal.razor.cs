using BlazorHero.CleanArchitecture.Application.Features.Brands.AddEdit;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class AddEditBrandModal : IDisposable
    {
        private bool success;
        private string[] errors = { };
        private MudForm form;

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        [Required]
        public string Name { get; set; }

        [Parameter]
        [Required]
        public decimal Tax { get; set; }

        [Parameter]
        [Required]
        public string Description { get; set; }

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        protected override void OnInitialized()
        {
            _interceptor.RegisterEvent();
        }
        public void Dispose() => _interceptor.DisposeEvent();
        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            form.Validate();
            if (form.IsValid)
            {
                var request = new AddEditBrandCommand() { Name = Name, Description = Description, Tax = Tax, Id = Id };
                var response = await _brandManager.SaveAsync(request);
                if (response.Succeeded)
                {
                    _snackBar.Add(response.Messages[0], Severity.Success);
                    MudDialog.Close();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        protected override async Task OnInitializedAsync() => await LoadDataAsync();

        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}