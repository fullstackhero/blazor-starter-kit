using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.Communication
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IChatService _chatService;
        public ChatsController(ICurrentUserService currentUserService, IChatService chatService)
        {
            _currentUserService = currentUserService;
            _chatService = chatService;
        }
        //Get user wise chat history

        //get available users - sorted by date of last message if exists
        [HttpGet("users")]
        public async Task<IActionResult> GetChatUsersAsync()
        {
            return Ok(await _chatService.GetChatUsersAsync(_currentUserService.UserId));
        }
        //save chat message
    }
}
