using AutoMapper;
using BlazorHero.CleanArchitecture.Domain.Entities.Identity;
using BlazorHero.CleanArchitecture.UserService.Interfaces.Responses;

namespace BlazorHero.CleanArchitecture.UserService.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserResponse, BlazorHeroUser>().ReverseMap();
        }
    }
}