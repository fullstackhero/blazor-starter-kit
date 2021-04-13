using BlazorHero.CleanArchitecture.Application.Models.Identity;
using System;

namespace BlazorHero.CleanArchitecture.Application.Models.Chat
{
    public partial class ChatHistory
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