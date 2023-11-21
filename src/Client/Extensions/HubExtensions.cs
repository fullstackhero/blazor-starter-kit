using Blazored.LocalStorage;
using CleanBlazor.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace CleanBlazor.Client.Extensions;

public static class HubExtensions
{
    public static HubConnection TryInitialize(
        this HubConnection hubConnection,
        NavigationManager navigationManager,
        ILocalStorageService localStorage) =>
        hubConnection ?? new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl),
                options =>
                {
                    options.AccessTokenProvider = async () => await localStorage.GetItemAsync<string>("authToken");
                })
            .WithAutomaticReconnect()
            .Build();

    public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager) =>
        hubConnection ?? new HubConnectionBuilder()
            .WithUrl(navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
            .Build();
}
