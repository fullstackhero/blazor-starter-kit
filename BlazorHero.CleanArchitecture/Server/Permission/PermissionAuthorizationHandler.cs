using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using BlazorHero.CleanArchitecture.Shared.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Permission
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {

        public PermissionAuthorizationHandler()
        { }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

            if (context.User == null) // roleclaims is not accessible - TODO: 
            {
                return;
            }
        
            var permissionss = context.User.Claims.Where(x => x.Type == "Permission" &&
                                                                x.Value == requirement.Permission &&
                                                                x.Issuer == "LOCAL AUTHORITY");
            if (permissionss.Any())
            {
                context.Succeed(requirement);
                return;
            }
        }
    }
}
