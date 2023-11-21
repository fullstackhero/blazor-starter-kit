using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Misc;

public partial class AddEditDocumentTypeModal
{
    private FluentValidationValidator _fluentValidationValidator;

    [Parameter] public AddEditDocumentTypeRequest AddEditDocumentTypeModel { get; set; } = new();
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [CascadingParameter] private HubConnection HubConnection { get; set; }
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    public void Cancel() => MudDialog.Cancel();

    private async Task SaveAsync()
    {
        await HttpClient.PostAsJsonAsync<AddEditDocumentTypeRequest, Result<int>>(
            DocumentTypesEndpoints.Save, AddEditDocumentTypeModel)
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
