using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using BlazorHero.CleanArchitecture.Application.Constants.Application;

namespace BlazorHero.CleanArchitecture.Client.Extensions
{
    public static class HubExtensions
    {
        public static HubConnection TryInitialize(this HubConnection hubConnection, NavigationManager navigationManager)
        {
            if (hubConnection == null)
            {
                hubConnection = new HubConnectionBuilder()
                                  .WithUrl(navigationManager.ToAbsoluteUri(ApplicationConstants.SignalR.HubUrl))
                                  .Build();
            }
            return hubConnection;
        }
    }
}