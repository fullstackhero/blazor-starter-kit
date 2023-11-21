using AutoMapper;
using CleanBlazor.Contracts.Identity;

namespace CleanBlazor.Client.Configuration.Mappings;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
        CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
    }
}
