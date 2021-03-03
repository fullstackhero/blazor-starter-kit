using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Responses.Roles;
using BlazorHero.CleanArchitecture.Client.ViewModels;

namespace BlazorHero.CleanArchitecture.Client.Mappings.Administrator
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, RoleViewModel>().ReverseMap();
        }
    }
}
