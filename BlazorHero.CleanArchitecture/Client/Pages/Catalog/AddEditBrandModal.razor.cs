using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Threading.Tasks;
using Blazored.FluentValidation;
using BlazorHero.CleanArchitecture.Application.Features.Brands.Commands.AddEdit;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class AddEditBrandModal
    {
        [Inject] private Microsoft.Extensions.Localization.IStringLocalizer<AddEditBrandModal> localizer { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        [Parameter]
        public AddEditBrandCommand AddEditBrandModel { get; set; } = new();

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] public HubConnection hubConnection { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SaveAsync()
        {
            var response = await _brandManager.SaveAsync(AddEditBrandModel);
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
            await hubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
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