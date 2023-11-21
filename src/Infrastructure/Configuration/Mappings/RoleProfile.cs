using AutoMapper;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Infrastructure.Models.Identity;

namespace CleanBlazor.Infrastructure.Configuration.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile() => CreateMap<RoleResponse, ApplicationRole>().ReverseMap();
}
