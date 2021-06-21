namespace BlazorHero.CleanArchitecture.Application.Models.Chat
{
    public class Message
    {
        public string ToUserId { get; set; }
        public string FromUserId { get; set; }
        public string MessageText { get; set; }
    }
}