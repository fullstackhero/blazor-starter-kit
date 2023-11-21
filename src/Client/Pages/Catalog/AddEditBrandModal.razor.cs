using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Catalog.Brands;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Catalog;

public partial class AddEditBrandModal
{
    private FluentValidationValidator _fluentValidationValidator;

    [Parameter] public AddEditBrandRequest AddEditBrandRequestModel { get; set; } = new();
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [CascadingParameter] private HubConnection HubConnection { get; set; }
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    public void Cancel() => MudDialog.Cancel();

    private async Task SaveAsync()
    {
        await HttpClient.PostAsJsonAsync<AddEditBrandRequest, Result<int>>(
                BrandsEndpoints.Save, AddEditBrandRequestModel)
            .Match((message, _) =>
                {
                    SnackBar.Success(message);
                    MudDialog.Close();
                },
                errors => SnackBar.Error(errors));

        await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
        HubConnection = HubConnection.TryInitialize(NavigationManager, LocalStorage);
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
    }

    private static async Task LoadDataAsync() => await Task.CompletedTask;
}
