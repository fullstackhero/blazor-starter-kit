using System;
using BlazorHero.CleanArchitecture.Domain.Entities.Identity;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Chat
{
    public partial class ChatHistory : IEntity
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual BlazorHeroUser FromUser { get; set; }
        public virtual BlazorHeroUser ToUser { get; set; }
    }
}