using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.ChatService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Domain.Entities.Chat;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Communication
{
    public interface IChatManager : IManager
    {
        Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync();

        Task<IResult> SaveMessageAsync(ChatHistory chatHistory);

        Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId);
    }
}