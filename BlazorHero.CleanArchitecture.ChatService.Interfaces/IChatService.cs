using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.ChatService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Domain.Entities.Chat;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.ChatService.Interfaces
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}