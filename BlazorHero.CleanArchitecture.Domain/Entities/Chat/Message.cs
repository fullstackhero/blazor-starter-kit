namespace BlazorHero.CleanArchitecture.Domain.Entities.Chat
{
    public class Message : IEntity
    {
        public string ToUserId { get; set; }
        public string FromUserId { get; set; }
        public string MessageText { get; set; }
    }
}