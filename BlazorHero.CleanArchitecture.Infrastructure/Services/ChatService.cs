using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private readonly BlazorHeroContext _context;
        private readonly IMapper _mapper;

        public ChatService(BlazorHeroContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId)
        {
            var allUsers = await _context.Users.Where(user=>user.Id != userId).ToListAsync();
            var chatUsers = _mapper.Map<IEnumerable<ChatUserResponse>>(allUsers);
            return Result<IEnumerable<ChatUserResponse>>.Success(chatUsers);
        }
    }
}
