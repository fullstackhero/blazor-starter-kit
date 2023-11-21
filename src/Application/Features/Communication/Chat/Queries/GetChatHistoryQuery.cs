using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Contracts.Chat;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.Communication.Chat.Queries;

public sealed record GetChatHistoryQuery(string ContactId) : IQuery<Result<IEnumerable<ChatMessageResponse>>>;

internal sealed class GetChatHistoryQueryHandler : IQueryHandler<GetChatHistoryQuery, Result<IEnumerable<ChatMessageResponse>>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserService _userService;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IStringLocalizer<GetChatHistoryQueryHandler> _localizer;

    public GetChatHistoryQueryHandler(
        ICurrentUserService currentUserService,
        IUserService userService,
        IChatMessageRepository chatMessageRepository,
        IStringLocalizer<GetChatHistoryQueryHandler> localizer)
    {
        _currentUserService = currentUserService;
        _userService = userService;
        _chatMessageRepository = chatMessageRepository;
        _localizer = localizer;
    }

    public async Task<Result<IEnumerable<ChatMessageResponse>>> Handle(
        GetChatHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var response = await _userService.GetAsync(_currentUserService.UserId);
        if (response.IsFailure)
        {
            return Result.Fail<IEnumerable<ChatMessageResponse>>(_localizer["User Not Found!"]);
        }

        List<ChatMessageResponse> chatMessages = await _chatMessageRepository.Entities
            .Where(h => (h.FromUserId == _currentUserService.UserId && h.ToUserId == request.ContactId) ||
                        (h.FromUserId == request.ContactId && h.ToUserId == _currentUserService.UserId))
            .OrderBy(a => a.CreatedDate)
            .Select(x => new ChatMessageResponse
            {
                FromUserId = x.FromUserId,
                FromUserFullName = $"{x.FromUser.FirstName} {x.FromUser.LastName}",
                Message = x.Message,
                CreatedDate = x.CreatedDate,
                Id = x.Id,
                ToUserId = x.ToUserId,
                ToUserFullName = $"{x.ToUser.FirstName} {x.ToUser.LastName}",
                ToUserImageUrl = x.ToUser.ProfilePictureDataUrl,
                FromUserImageUrl = x.FromUser.ProfilePictureDataUrl
            }).ToListAsync(cancellationToken);

        return chatMessages;
    }
}
