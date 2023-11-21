using AutoMapper;
using CleanBlazor.Contracts.Chat;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Infrastructure.Models.Identity;

namespace CleanBlazor.Infrastructure.Configuration.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserResponse, ApplicationUser>().ReverseMap();
        CreateMap<ChatUserResponse, ApplicationUser>().ReverseMap()
            .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(source => source.Email)); //Specific Mapping
    }
}
