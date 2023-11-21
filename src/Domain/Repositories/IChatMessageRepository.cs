using CleanBlazor.Domain.Entities.Communication;

namespace CleanBlazor.Domain.Repositories;

public interface IChatMessageRepository : IRepository<ChatMessage<IChatUser>, long>
{

}
