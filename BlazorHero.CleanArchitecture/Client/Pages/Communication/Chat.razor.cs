using AdminDashboard.Wasm.Models;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Communication
{
    public partial class Chat
    {
        private HubConnection hubConnection;
        [Parameter] public string CurrentMessage { get; set; }
        private bool isConnected => hubConnection.State == HubConnectionState.Connected;
        private List<ChatMessage> messages = new List<ChatMessage>();
        private class MessageRequest
        {
            public string userName { get; set; }
            public string message { get; set; }
        }
        private class ChatMessage
        {
            public string userName { get; set; }
            public string message { get; set; }
        }
        MessageRequest model = new MessageRequest();
        private async Task SubmitAsync()
        {
            if(!string.IsNullOrEmpty(CurrentMessage))
            {
                var state = await _stateProvider.GetAuthenticationStateAsync();
                var user = state.User;
                var UserId = user.GetUserId();
                var userName = $"{user.GetFirstName()} {user.GetLastName()}";
                await hubConnection.SendAsync("SendMessageAsync", userName, CurrentMessage);
                CurrentMessage = string.Empty;
            }            
        }
        private async Task OnKeyPressInChat(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await SubmitAsync();
            }
        }
        protected override async Task OnInitializedAsync()
        {
            //ideally dont get user from db directly, rather get from the chat history so that it's sorted by the last chat time/date.
            
            hubConnection = new HubConnectionBuilder().WithUrl(_navigationManager.ToAbsoluteUri("/chatHub"))
            .Build();
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                messages.Add(new ChatMessage { message = message, userName = user });
                StateHasChanged();
            });
            await hubConnection.StartAsync();
            await GetUsersAsync();
        }
        public List<UserResponse> UserList = new List<UserResponse>();
        [Parameter] public string CurrentlyChattingWith { get; set; }
        void LoadUserChat(string userName)
        {
            //Clear Chat
            messages = new List<ChatMessage>();
            CurrentlyChattingWith = userName;
        }
        private async Task GetUsersAsync()
        {
            //add get chat history from chat controller / manager
            var response = await _userManager.GetAllAsync();
            if (response.Succeeded)
            {
                UserList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(localizer[message], Severity.Error);
                }
            }
        }

        bool open;
        Anchor ChatDrawer { get; set; }
        void OpenDrawer(Anchor anchor)
        {
            ChatDrawer = anchor;
            open = true;
        }
    }
}
