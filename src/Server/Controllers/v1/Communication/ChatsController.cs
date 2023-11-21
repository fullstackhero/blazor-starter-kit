using CleanBlazor.Application.Features.Communication.Chat.Commands;
using CleanBlazor.Application.Features.Communication.Chat.Queries;
using CleanBlazor.Domain.Entities.Communication;
using CleanBlazor.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.Communication;

[Authorize(Policy = Permissions.Communication.Chat)]
public class ChatsController : BaseApiController
{
    /// <summary>
    ///     Get user wise chat history
    /// </summary>
    /// <param name="contactId"></param>
    /// <returns>Status 200 OK</returns>
    //Get user wise chat history
    [HttpGet("{contactId}")]
    public async Task<IActionResult> GetChatHistoryAsync(string contactId) =>
        Ok(await Sender.Send(new GetChatHistoryQuery(contactId)));

    /// <summary>
    ///     get available users
    /// </summary>
    /// <returns>Status 200 OK</returns>
    //get available users - sorted by date of last message if exists
    [HttpGet("users")]
    public async Task<IActionResult> GetChatUsersAsync() =>
        Ok(await Sender.Send(new GetChatUsersQuery()));

    /// <summary>
    ///     Save Chat Message
    /// </summary>
    /// <param name="message"></param>
    /// <returns>Status 200 OK</returns>
    //save chat message
    [HttpPost]
    public async Task<IActionResult> SaveMessageAsync(ChatMessage<IChatUser> message) =>
        Ok(await Sender.Send(new SaveChatMessageCommand(message.ToUserId, message.Message)));
}
