using AutoMapper;
using BlazorHero.CleanArchitecture.Shared.Responses.Roles;
using Microsoft.AspNetCore.Identity;

namespace BlazorHero.CleanArchitecture.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, IdentityRole>().ReverseMap();
        }
    }
}
