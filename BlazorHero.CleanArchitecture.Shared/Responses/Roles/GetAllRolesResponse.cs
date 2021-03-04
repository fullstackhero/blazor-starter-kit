using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Shared.Responses.Roles
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}
