using AutoMapper;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Chat;

namespace CleanBlazor.Application.Configuration.Mappings;

public class ChatProfile : Profile
{
    public ChatProfile()
    {
        CreateMap<ChatUserResponse, IUser>().ReverseMap()
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(source => source.Email)); //Specific Mapping
    }
}
