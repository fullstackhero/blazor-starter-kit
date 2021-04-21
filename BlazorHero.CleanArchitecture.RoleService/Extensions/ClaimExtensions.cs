using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Constants.Permission;
using BlazorHero.CleanArchitecture.RoleService.Interfaces.Responses;

namespace BlazorHero.CleanArchitecture.RoleService.Extensions
{
    public static class ClaimExtensions
    {
        public static void GetPermissions(this List<RoleClaimsResponse> allPermissions, Type policy, string roleId)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimsResponse { Value = fi.GetValue(null).ToString(), Type = ApplicationClaimTypes.Permission });
            }
        }

        public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
            }
        }
    }
}