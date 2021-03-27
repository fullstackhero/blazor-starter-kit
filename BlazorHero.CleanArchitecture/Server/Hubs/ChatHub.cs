using BlazorHero.CleanArchitecture.Application.Models.Chat;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessageAsync(ChatHistory chatHistory, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", chatHistory, userName);
        }
        // hubConnection.SendAsync("SendNotificationAsync", $"New Message From {userName}", Severity.Normal);
        public async Task ChatNotificationAsync(string message, string severity)
        {
            await Clients.All.SendAsync("ReceiveChatNotification", message, severity);
        }
    }
}
