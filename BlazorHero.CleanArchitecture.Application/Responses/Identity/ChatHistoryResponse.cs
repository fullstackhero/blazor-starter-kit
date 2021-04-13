using System;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public partial class ChatHistoryResponse
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string FromUserImageURL { get; set; }
        public string FromUserFullName { get; set; }
        public string ToUserId { get; set; }
        public string ToUserImageURL { get; set; }
        public string ToUserFullName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}