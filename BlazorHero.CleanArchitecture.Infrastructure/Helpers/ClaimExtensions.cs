using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;

namespace BlazorHero.CleanArchitecture.Infrastructure.Helpers
{
    public static class ClaimsHelper
    {
        public static void AddPermissions(this List<RoleClaimResponse> allPermissions, Type policy)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimResponse { Value = fi.GetValue(null)?.ToString(), Type = ApplicationClaimTypes.Permission, Group = policy.Name });
            }
        }

        public static async Task<IdentityResult> AddPermissionClaim(this RoleManager<BlazorHeroRole> roleManager, BlazorHeroRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
            {
                return await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
            }

            return IdentityResult.Failed();
        }

        public static async Task GeneratePermissionClaimByModule(this RoleManager<BlazorHeroRole> roleManager, BlazorHeroRole role, string module)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            var allPermissions = PermissionModules.GeneratePermissionsForModule(module);
            foreach (var permission in allPermissions)
            {
                if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
                {
                    await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
                }
            }
        }

        public static async Task AddCustomPermissionClaim(this RoleManager<BlazorHeroRole> roleManager, BlazorHeroRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
            }
        }
    }
}