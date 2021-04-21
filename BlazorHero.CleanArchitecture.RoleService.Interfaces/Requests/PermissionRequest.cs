using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.RoleService.Interfaces.Requests
{
    public class PermissionRequest
    {
        public string RoleId { get; set; }
        public IList<RoleClaimsRequest> RoleClaims { get; set; }
    }
}