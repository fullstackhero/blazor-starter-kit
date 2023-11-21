using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanBlazor.Domain.Entities.Communication;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Infrastructure.Data;
using CleanBlazor.Infrastructure.Models.Identity;

namespace CleanBlazor.Infrastructure.Repositories;

internal sealed class ChatMessageRepository : GenericRepository<ChatMessage<IChatUser>, long>, IChatMessageRepository
{
    private readonly IMapper _mapper;

    public ChatMessageRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    public override IQueryable<ChatMessage<IChatUser>> Entities => DbContext.Set<ChatMessage<ApplicationUser>>()
        .ProjectTo<ChatMessage<IChatUser>>(_mapper.ConfigurationProvider);

    public override ChatMessage<IChatUser> Add(ChatMessage<IChatUser> entity)
    {
        var mapped = _mapper.Map<ChatMessage<ApplicationUser>>(entity);
        var added = DbContext.Set<ChatMessage<ApplicationUser>>().Add(mapped).Entity;
        return _mapper.Map<ChatMessage<IChatUser>>(added);
    }
}
