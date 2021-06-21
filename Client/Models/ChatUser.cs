using MudBlazor;

namespace AdminDashboard.Wasm.Models
{
    public class ChatUser
    {
        public string UserName { get; set; }
        public string UserRoleColor { get; set; }
        public Color OnlineStatus { get; set; }
        public bool Spotify { get; set; }
        public string AvatarUrl { get; set; }
        public Color AvatarColor { get; set; }
    }
}