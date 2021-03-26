using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class ChatEndpoint
    {
        public static string GetAvailableUsers = "api/chats/users";
        public static string SaveMessage = "api/chats";
        public static string GetChatHistory(string userId)
        {
            return $"api/chats/{userId}";
        }
    }
}
