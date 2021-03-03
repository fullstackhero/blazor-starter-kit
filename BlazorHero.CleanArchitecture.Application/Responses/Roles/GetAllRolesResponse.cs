using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Application.Responses.Roles
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}
