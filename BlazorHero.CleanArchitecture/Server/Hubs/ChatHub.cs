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
    }
}
