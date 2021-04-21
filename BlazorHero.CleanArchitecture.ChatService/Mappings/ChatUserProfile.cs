using AutoMapper;
using BlazorHero.CleanArchitecture.ChatService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Domain.Entities.Identity;

namespace BlazorHero.CleanArchitecture.ChatService.Mappings
{
    public class ChatUserProfile : Profile
    {
        public ChatUserProfile()
        {
            CreateMap<ChatUserResponse, BlazorHeroUser>().ReverseMap()
                .ForMember(dest => dest.EmailAddress, source => source.MapFrom(source => source.Email)); //Specific Mapping
        }
    }
}
