using BlazorHero.CleanArchitecture.Application.Models.Chat;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Interfaces.Chat;
using BlazorHero.CleanArchitecture.Shared.Constants.Storage;

namespace BlazorHero.CleanArchitecture.Client.Pages.Communication
{
    public partial class Chat
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public string CurrentMessage { get; set; }
        [Parameter] public string CurrentUserId { get; set; }
        [Parameter] public string CurrentUserImageURL { get; set; }

        private List<ChatHistoryResponse> _messages = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
        }

        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(CurrentMessage) && !string.IsNullOrEmpty(CId))
            {
                //Save Message to DB
                var chatHistory = new ChatHistory<IChatUser>
                {
                    Message = CurrentMessage,
                    ToUserId = CId,
                    CreatedDate = DateTime.Now
                };
                var response = await _chatManager.SaveMessageAsync(chatHistory);
                if (response.Succeeded)
                {
                    var state = await _stateProvider.GetAuthenticationStateAsync();
                    var user = state.User;
                    CurrentUserId = user.GetUserId();
                    chatHistory.FromUserId = CurrentUserId;
                    var userName = $"{user.GetFirstName()} {user.GetLastName()}";
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendMessage, chatHistory, userName);
                    CurrentMessage = string.Empty;
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task OnKeyPressInChat(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SubmitAsync();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            HubConnection.On<string>(ApplicationConstants.SignalR.ConnectUser, (userId) =>
            {
                var connectedUser = UserList.Find(x => x.Id.Equals(userId));
                if (connectedUser is {IsOnline: false})
                {
                    connectedUser.IsOnline = true;
                    _snackBar.Add($"{connectedUser.UserName} {_localizer["Logged In."]}", Severity.Info);
                    StateHasChanged();
                }
            });
            HubConnection.On<string>(ApplicationConstants.SignalR.DisconnectUser, (userId) =>
            {
                var disconnectedUser = UserList.Find(x => x.Id.Equals(userId));
                if (disconnectedUser is {IsOnline: true})
                {
                    disconnectedUser.IsOnline = false;
                    _snackBar.Add($"{disconnectedUser.UserName} {_localizer["Logged Out."]}", Severity.Info);
                    StateHasChanged();
                }
            });
            HubConnection.On<ChatHistory<IChatUser>, string>(ApplicationConstants.SignalR.ReceiveMessage, async (chatHistory, userName) =>
             {
                 if ((CId == chatHistory.ToUserId && CurrentUserId == chatHistory.FromUserId) || (CId == chatHistory.FromUserId && CurrentUserId == chatHistory.ToUserId))
                 {
                     if ((CId == chatHistory.ToUserId && CurrentUserId == chatHistory.FromUserId))
                     {
                         _messages.Add(new ChatHistoryResponse { Message = chatHistory.Message, FromUserFullName = userName, CreatedDate = chatHistory.CreatedDate, FromUserImageURL = CurrentUserImageURL });
                         await HubConnection.SendAsync(ApplicationConstants.SignalR.SendChatNotification, string.Format(_localizer["New Message From {0}"], userName), CId, CurrentUserId);
                     }
                     else if ((CId == chatHistory.FromUserId && CurrentUserId == chatHistory.ToUserId))
                     {
                         _messages.Add(new ChatHistoryResponse { Message = chatHistory.Message, FromUserFullName = userName, CreatedDate = chatHistory.CreatedDate, FromUserImageURL = CImageURL });
                     }
                     await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
                     StateHasChanged();
                 }
             });
            await GetUsersAsync();
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            CurrentUserId = user.GetUserId();
            CurrentUserImageURL = await _localStorage.GetItemAsync<string>(StorageConstants.Local.UserImageURL);
            if (!string.IsNullOrEmpty(CId))
            {
                await LoadUserChat(CId);
            }
        }

        public List<ChatUserResponse> UserList = new();
        [Parameter] public string CFullName { get; set; }
        [Parameter] public string CId { get; set; }
        [Parameter] public string CUserName { get; set; }
        [Parameter] public string CImageURL { get; set; }

        private async Task LoadUserChat(string userId)
        {
            _open = false;
            var response = await _userManager.GetAsync(userId);
            if (response.Succeeded)
            {
                var contact = response.Data;
                CId = contact.Id;
                CFullName = $"{contact.FirstName} {contact.LastName}";
                CUserName = contact.UserName;
                CImageURL = contact.ProfilePictureDataUrl;
                _navigationManager.NavigateTo($"chat/{CId}");
                //Load messages from db here
                _messages = new List<ChatHistoryResponse>();
                var historyResponse = await _chatManager.GetChatHistoryAsync(CId);
                if (historyResponse.Succeeded)
                {
                    _messages = historyResponse.Data.ToList();
                }
                else
                {
                    foreach (var message in historyResponse.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task GetUsersAsync()
        {
            //add get chat history from chat controller / manager
            var response = await _chatManager.GetChatUsersAsync();
            if (response.Succeeded)
            {
                UserList = response.Data.ToList();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private bool _open;
        private Anchor ChatDrawer { get; set; }

        private void OpenDrawer(Anchor anchor)
        {
            ChatDrawer = anchor;
            _open = true;
        }

        private Color GetUserStatusBadgeColor(bool isOnline)
        {
            switch (isOnline)
            {
                case false:
                    return Color.Error;
                case true:
                    return Color.Success;
            }
        }
    }
}