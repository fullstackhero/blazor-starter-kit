using AutoMapper;
using BlazorHero.CleanArchitecture.RoleService.Interfaces.Requests;
using BlazorHero.CleanArchitecture.RoleService.Interfaces.Responses;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimsResponse, RoleClaimsRequest>().ReverseMap();
        }
    }
}