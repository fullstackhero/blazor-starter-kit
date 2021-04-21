using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.ChatService.Interfaces;
using BlazorHero.CleanArchitecture.ChatService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.DataAccess.Interfaces.Contexts;
using BlazorHero.CleanArchitecture.Domain.Entities.Chat;
using BlazorHero.CleanArchitecture.UserService.Interfaces;
using BlazorHero.CleanArchitecture.Utils.Exceptions;
using BlazorHero.CleanArchitecture.Utils.Wrapper;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.ChatService
{
    public class ChatService : IChatService
    {
        private readonly IBlazorHeroContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IStringLocalizer<ChatService> _localizer;

        public ChatService(
            IBlazorHeroContext context,
            IMapper mapper,
            IUserService userService,
            IStringLocalizer<ChatService> localizer)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _localizer = localizer;
        }

        public async Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId)
        {
            var response = await _userService.GetAsync(userId);
            if (response.Succeeded)
            {
                var user = response.Data;
                var query = await _context.ChatHistories
                    .Where(h => (h.FromUserId == userId && h.ToUserId == contactId) || (h.FromUserId == contactId && h.ToUserId == userId))
                    .OrderBy(a => a.CreatedDate)
                    .Include(a => a.FromUser)
                    .Include(a => a.ToUser)
                    .Select(x => new ChatHistoryResponse
                    {
                        FromUserId = x.FromUserId,
                        FromUserFullName = $"{x.FromUser.FirstName} {x.FromUser.LastName}",
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ToUserId = x.ToUserId,
                        ToUserFullName = $"{x.ToUser.FirstName} {x.ToUser.LastName}",
                        ToUserImageURL = x.ToUser.ProfilePictureDataUrl,
                        FromUserImageURL = x.FromUser.ProfilePictureDataUrl
                    }).ToListAsync();
                return await Result<IEnumerable<ChatHistoryResponse>>.SuccessAsync(query);
            }
            else
            {
                throw new ApiException(_localizer["User Not Found!"]);
            }
        }

        public async Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId)
        {
            var allUsers = await _context.Users.Where(user => user.Id != userId).ToListAsync();
            var chatUsers = _mapper.Map<IEnumerable<ChatUserResponse>>(allUsers);
            return await Result<IEnumerable<ChatUserResponse>>.SuccessAsync(chatUsers);
        }

        public async Task<IResult> SaveMessageAsync(ChatHistory message)
        {
            message.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
            await _context.ChatHistories.AddAsync(message);
            await _context.SaveChangesAsync();
            return await Result.SuccessAsync();
        }
    }
}