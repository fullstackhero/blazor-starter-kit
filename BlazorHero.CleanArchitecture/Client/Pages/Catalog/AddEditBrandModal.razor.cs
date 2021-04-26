using BlazorHero.CleanArchitecture.Application.Features.Brands.AddEdit;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class AddEditBrandModal
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
        [CascadingParameter] public HubConnection hubConnection { get; set; }

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
                    _snackBar.Add(localizer[response.Messages[0]], Severity.Success);
                    MudDialog.Close();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(localizer[message], Severity.Error);
                    }
                }
                await hubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
            hubConnection = hubConnection.TryInitialize(_navigationManager);
            if (hubConnection.State == HubConnectionState.Disconnected)
            {
                await hubConnection.StartAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            await Task.CompletedTask;
        }
    }
}