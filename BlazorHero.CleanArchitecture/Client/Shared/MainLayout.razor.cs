using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Settings;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Shared
{
    public partial class MainLayout : IDisposable
    {
        private string CurrentUserId { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }
        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity.IsAuthenticated)
            {
                CurrentUserId = user.GetUserId();
                this.FirstName = user.GetFirstName();
                if (this.FirstName.Length > 0)
                {
                    FirstLetterOfName = FirstName[0];
                }

            }

        }
        MudTheme currentTheme;
        private bool _drawerOpen = true;
        protected override async Task OnInitializedAsync()
        {
            _interceptor.RegisterEvent();
            currentTheme = await _preferenceManager.GetCurrentThemeAsync();

            hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/chatHub"))
            .Build();
            hubConnection.On<string, string>("ReceiveChatNotification", (message, userId) =>
            {
                if (CurrentUserId == userId)
                {
                    _snackBar.Add(message, Severity.Info, options => options.BackgroundBlurred = true);
                }
            });
            await hubConnection.StartAsync();
        }
        void Logout()
        {
            string logoutConfirmationText = localizer["Logout Confirmation"];
            string logoutText = localizer["Logout"];
            var parameters = new DialogParameters();
            parameters.Add("ContentText", logoutConfirmationText);
            parameters.Add("ButtonText", logoutText);
            parameters.Add("Color", Color.Error);

            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            _dialogService.Show<Dialogs.Logout>("Logout", parameters, options);
        }
        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
        async Task DarkMode()
        {
            bool isDarkMode = await _preferenceManager.ToggleDarkModeAsync();
            if (isDarkMode)
            {
                currentTheme = BlazorHeroTheme.DefaultTheme;
            }
            else
            {
                currentTheme = BlazorHeroTheme.DarkTheme;
            }
        }
        public void Dispose()
        {
            _interceptor.DisposeEvent();
            _ = hubConnection.DisposeAsync();
        }

        private HubConnection hubConnection;
        public bool IsConnected =>
        hubConnection.State == HubConnectionState.Connected;
    }
}
