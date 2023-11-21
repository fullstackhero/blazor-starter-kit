using System.Net.Http.Headers;
using System.Security.Claims;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Client.Shared.Dialogs;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

namespace CleanBlazor.Client.Shared;

public partial class MainBody
{
    private bool _drawerOpen = true;
    private bool _rightToLeft = false;

    private HubConnection _hubConnection;

    [Parameter] public RenderFragment ChildContent { get; set; }

    [Parameter] public EventCallback OnDarkModeToggle { get; set; }

    [Parameter] public EventCallback<bool> OnRightToLeftToggle { get; set; }

    [Inject] private ILogger<MainBody> Logger { get; set; }

    private string CurrentUserId { get; set; }
    private string ImageDataUrl { get; set; }
    private string FirstName { get; set; }
    private string SecondName { get; set; }
    private string Email { get; set; }
    private char FirstLetterOfName { get; set; }
    public bool IsConnected => _hubConnection.State == HubConnectionState.Connected;

    private async Task RightToLeftToggle()
    {
        var isRtl = await ClientPreferenceManager.ToggleLayoutDirection();
        _rightToLeft = isRtl;

        await OnRightToLeftToggle.InvokeAsync(isRtl);
    }

    public async Task ToggleDarkMode() => await OnDarkModeToggle.InvokeAsync();

    protected override async Task OnInitializedAsync()
    {
        _rightToLeft = await ClientPreferenceManager.IsRtl();
        _hubConnection = _hubConnection.TryInitialize(NavigationManager, LocalStorage);
        await _hubConnection.StartAsync();
        _hubConnection.On<string, string, string>(ApplicationConstants.SignalR.ReceiveChatNotification,
            async (message, receiverUserId, senderUserId) =>
            {
                if (CurrentUserId != receiverUserId)
                {
                    return;
                }

                await JsRuntime.InvokeAsync<string>("PlayAudio", "notification");
                SnackBar.Add(message,
                    Severity.Info,
                    config =>
                    {
                        config.VisibleStateDuration = 10000;
                        config.HideTransitionDuration = 500;
                        config.ShowTransitionDuration = 500;
                        config.Action = Localizer["Chat?"];
                        config.ActionColor = Color.Primary;
                        config.Onclick = _ =>
                        {
                            NavigationManager.NavigateTo($"chat/{senderUserId}");
                            return Task.CompletedTask;
                        };
                    });
            });

        _hubConnection.On<string, string>(ApplicationConstants.SignalR.LogoutUsersByRole,
            async (userId, roleId) =>
            {
                if (CurrentUserId != userId)
                {
                    var rolesResult = await HttpClient.GetFromJsonAsync<Result<List<RoleResponse>>>(RolesEndpoints.GetAll);
                    if (rolesResult.IsSuccess)
                    {
                        RoleResponse role = rolesResult.Data.FirstOrDefault(x => x.Id == roleId);
                        if (role != null)
                        {
                            var userRolesResult = await HttpClient.GetFromJsonAsync<Result<UserRolesResponse>>(
                                UsersEndpoints.GetUserRolesById(CurrentUserId));
                            if (userRolesResult.IsSuccess &&
                                userRolesResult.Data.UserRoles.Any(x => x.RoleName == role.Name))
                            {
                                await _hubConnection.SendAsync(ApplicationConstants.SignalR.OnDisconnect,
                                    CurrentUserId);
                                await AuthenticationManager.LogoutAsync();
                                SnackBar.Error(
                                    Localizer[
                                        "You are logged out because the Permissions of one of your Roles have been updated."]);
                            }
                        }
                    }
                }
            });
        _hubConnection.On<string>(ApplicationConstants.SignalR.PingRequest,
            async userName =>
            {
                await _hubConnection.SendAsync(ApplicationConstants.SignalR.PingResponse, CurrentUserId, userName);
            });

        await _hubConnection.SendAsync(ApplicationConstants.SignalR.OnConnect, CurrentUserId);

        SnackBar.Success(string.Format(Localizer["Welcome {0}"], FirstName));
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadDataAsync();
        }
    }

    private async Task LoadDataAsync()
    {
        ClaimsPrincipal user = await StateProvider.GetCurrentUserAsync();
        if (user == null)
        {
            return;
        }

        if (user.Identity?.IsAuthenticated == true)
        {
            if (string.IsNullOrEmpty(CurrentUserId))
            {
                CurrentUserId = user.GetUserId();
                FirstName = user.GetFirstName();
                if (FirstName.Length > 0)
                {
                    FirstLetterOfName = FirstName[0];
                }

                SecondName = user.GetLastName();
                Email = user.GetEmail();
                await HttpClient.GetFromJsonAsync<Result<string>>(AccountsEndpoints.GetProfilePicture(CurrentUserId))
                    .Match((_, imageData) => ImageDataUrl = imageData, _ => { });

                var result =
                    await HttpClient.GetFromJsonAsync<Result<UserResponse>>(UsersEndpoints.GetById(CurrentUserId));
                if (result.IsFailure || result.Data == null)
                {
                    CurrentUserId = string.Empty;
                    ImageDataUrl = string.Empty;
                    FirstName = string.Empty;
                    SecondName = string.Empty;
                    Email = string.Empty;
                    FirstLetterOfName = char.MinValue;
                    await AuthenticationManager.LogoutAsync();
                    SnackBar.Error(
                        Localizer["You are logged out because the user with your Token has been deleted."]);
                }
            }
        }
    }

    private void DrawerToggle() => _drawerOpen = !_drawerOpen;

    private void Logout()
    {
        var parameters = new DialogParameters
        {
            { nameof(Dialogs.Logout.ContentText), $"{Localizer["Logout Confirmation"]}" },
            { nameof(Dialogs.Logout.ButtonText), $"{Localizer["Logout"]}" },
            { nameof(Dialogs.Logout.Color), Color.Error },
            { nameof(Dialogs.Logout.CurrentUserId), CurrentUserId },
            { nameof(Dialogs.Logout.HubConnection), _hubConnection }
        };

        var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

        DialogService.Show<Logout>(Localizer["Logout"], parameters, options);
    }
}
