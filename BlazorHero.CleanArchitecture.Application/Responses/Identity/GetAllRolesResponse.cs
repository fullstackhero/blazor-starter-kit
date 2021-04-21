using System.Collections.Generic;
using BlazorHero.CleanArchitecture.RoleService.Interfaces.Responses;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}