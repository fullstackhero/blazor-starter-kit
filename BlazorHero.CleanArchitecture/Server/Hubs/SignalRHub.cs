using BlazorHero.CleanArchitecture.Application.Models.Chat;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Hubs
{
    public class SignalRHub : Hub
    {
        public async Task SendMessageAsync(ChatHistory chatHistory, string userName)
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveMessage, chatHistory, userName);
        }

        public async Task ChatNotificationAsync(string message, string receiverUserId, string senderUserId)
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveChatNotification, message, receiverUserId, senderUserId);
        }

        public async Task UpdateDashboardAsync()
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveUpdateDashboard);
        }

        public async Task RegenerateTokensAsync()
        {
            await Clients.All.SendAsync(ApplicationConstants.SignalR.ReceiveRegenerateTokens);
        }
    }
}