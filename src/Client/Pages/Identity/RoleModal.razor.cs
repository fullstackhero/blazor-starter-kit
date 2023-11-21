using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Identity;

public partial class RoleModal
{
    private FluentValidationValidator _fluentValidationValidator;

    [Parameter] public RoleRequest RoleModel { get; set; } = new();
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
    [CascadingParameter] private HubConnection HubConnection { get; set; }
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    public void Cancel() => MudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
        HubConnection = HubConnection.TryInitialize(NavigationManager, LocalStorage);
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
    }

    private async Task SaveAsync() =>
        await HttpClient.PostAsJsonAsync<RoleRequest, Result<string>>(RolesEndpoints.Save, RoleModel)
            .Match(async (message, _) =>
                {
                    SnackBar.Success(message);
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    MudDialog.Close();
                },
                errors => SnackBar.Error(errors));
}
