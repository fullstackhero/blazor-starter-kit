using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.RoleService.Interfaces.Responses
{
    public class PermissionResponse
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RoleClaimsResponse> RoleClaims { get; set; }
    }
}