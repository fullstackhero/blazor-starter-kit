using AutoMapper;
using BlazorHero.CleanArchitecture.RoleService.Interfaces.Responses;
using Microsoft.AspNetCore.Identity;

namespace BlazorHero.CleanArchitecture.RoleService.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, IdentityRole>().ReverseMap();
        }
    }
}