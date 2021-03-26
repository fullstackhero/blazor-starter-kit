using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public partial class ChatHistoryResponse
    {
        public long Id { get; set; }
        public string FromUserId { get; set; }
        public string FromUserFullName { get; set; }
        public string ToUserId { get; set; }
        public string ToUserFullName { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
