using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Shared.Responses.Identity
{
    public class GetAllRolesResponse
    {
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}
