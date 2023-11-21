using AutoMapper;
using CleanBlazor.Domain.Entities.Communication;
using CleanBlazor.Infrastructure.Models.Identity;

namespace CleanBlazor.Infrastructure.Configuration.Mappings;

public class ChatProfile : Profile
{
    public ChatProfile() => CreateMap<ChatMessage<IChatUser>, ChatMessage<ApplicationUser>>()
        .ForMember(dest => dest.DomainEvents, opt => opt.Ignore())
        .ReverseMap()
        .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
}
