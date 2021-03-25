using AdminDashboard.Wasm.Models;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Communication
{
    public partial class Chat
    {
        public List<UserResponse> UserList = new List<UserResponse>();
        [Parameter] public string CurrentlyChattingWith { get; set; }
        void LoadUserChat(string userName)
        {
            //Clear Chat
            chatMessages = new ChatMessage[] { };
            CurrentlyChattingWith = userName;
        }
        protected override async Task OnInitializedAsync()
        {
            //ideally dont get user from db directly, rather get from the chat history so that it's sorted by the last chat time/date.
            await GetUsersAsync();
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

        //Get all available contacts from identity service
        ChatUser[] chatUsers = new ChatUser[]
    {
        new ChatUser { UserName = "Garderoben", UserRoleColor = Colors.DeepPurple.Accent4, OnlineStatus = Color.Success, Spotify = true, AvatarUrl = "https://avatars2.githubusercontent.com/u/10367109?s=460&amp;u=2abf95f9e01132e8e2915def42895ffe99c5d2c6&amp;v=4"},
        new ChatUser { UserName = "Henon", UserRoleColor = Colors.DeepPurple.Accent4, OnlineStatus = Color.Success, Spotify = false, AvatarUrl = "https://avatars.githubusercontent.com/u/44090?s=460&v=4"},
        new ChatUser { UserName = "Flaflo", UserRoleColor = Colors.Red.Accent3, OnlineStatus = Color.Success, Spotify = true, AvatarUrl = "https://avatars.githubusercontent.com/u/12973684?s=460&u=ea557f04c5d9c54f902f8c700292baefe59217d0&v=4"},
        new ChatUser { UserName = "porkopek", UserRoleColor = Colors.Red.Accent3, OnlineStatus = Color.Warning, Spotify = false, AvatarUrl = "https://avatars.githubusercontent.com/u/13745954?s=460&u=81ef9118f63113ad64bde35add178cbd9ca3bb38&v=4"},
        new ChatUser { UserName = "mike-gh", UserRoleColor = Colors.Red.Accent3, OnlineStatus = Color.Warning, Spotify = false, AvatarColor = Color.Success},
        new ChatUser { UserName = "tungi52", UserRoleColor = Colors.Red.Accent3, OnlineStatus = Color.Warning, Spotify = false, AvatarColor = Color.Success},
        new ChatUser { UserName = "svenovic", UserRoleColor = Colors.Red.Accent3, OnlineStatus = Color.Dark, Spotify = false, AvatarColor = Color.Success},
        new ChatUser { UserName = "Artroxa", UserRoleColor = Colors.BlueGrey.Lighten1, OnlineStatus = Color.Success, Spotify = false, AvatarUrl = "https://avatars2.githubusercontent.com/u/71094850?s=460&u=66c16f5bb7d27dc751f6759a82a3a070c8c7fe4b&v=4"},
        new ChatUser { UserName = "II ARROWS", UserRoleColor = Colors.BlueGrey.Lighten1, OnlineStatus = Color.Success, Spotify = false, AvatarUrl = "https://avatars.githubusercontent.com/u/14835013?s=460&u=8d9acfca411be6941ceb44f710c4357857350c2a&v=4"},
        new ChatUser { UserName = "rangsk", UserRoleColor = Colors.BlueGrey.Lighten1, OnlineStatus = Color.Success, Spotify = false, AvatarUrl = "https://avatars.githubusercontent.com/u/10701249?s=460&u=f806998af6e29fd4402736ba2efc2649adae9e39&v=4"},
        new ChatUser { UserName = "Svarta Änkan", UserRoleColor = Colors.BlueGrey.Lighten1, OnlineStatus = Color.Error, Spotify = true, AvatarColor = Color.Error},
        new ChatUser { UserName = "TommyG", UserRoleColor = Colors.BlueGrey.Lighten1, OnlineStatus = Color.Warning, Spotify = false, AvatarUrl = "https://avatars.githubusercontent.com/u/4773183?s=460&u=7d0ebb28e29ae5103a74070471ffd506cdbf03fd&v=4"},
                };
        //chat history from each user to currently logged in user

        ChatMessage[] chatMessages = new ChatMessage[]
        {
        new ChatMessage { UserName = "Garderoben", Message = "What is CSS?"},
        new ChatMessage { UserName = "Henon", Message = "idk?"},
        new ChatMessage { UserName = "Garderoben", Message = "me neither, anyone else?"},
        new ChatMessage { UserName = "Artroxa", Message = "lololololol"},
        new ChatMessage { UserName = "svenovic", Message = "Cascading Style Sheets"},
        new ChatMessage { UserName = "rangsk", Message = "CSS is a style sheet language used for describing the presentation of a document written in a markup language such as HTML."},
        new ChatMessage { UserName = "II ARROWS", Message = "We're on a mission from Glod."},
        new ChatMessage { UserName = "TommyG", Message = "Wth dude, i love Terry Pratchett"},
        new ChatMessage { UserName = "tungi52", Message = "Did you guys read his discworld book series?"},
        new ChatMessage { UserName = "TommyG", Message = "Yeah! It's great!"},
        new ChatMessage { UserName = "Garderoben", Message = "no more talk about books, this is very serious coder chat!"},
        new ChatMessage { UserName = "Artroxa", Message = "Yes, blazor is cool coding for cool kids" },
        new ChatMessage { UserName = "Flaflo", Message = "@Garderoben hey have you even started working on issue #294 on git?"},
        new ChatMessage { UserName = "Henon", Message = "Don't worry Flaflo, i took care of it."},
                    };

        public ChatUser GetUser(string username)
        {
            var chatUser = chatUsers.FirstOrDefault(x => x.UserName == username);
            return chatUser;
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
