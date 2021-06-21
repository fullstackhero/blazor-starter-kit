using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Application.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}