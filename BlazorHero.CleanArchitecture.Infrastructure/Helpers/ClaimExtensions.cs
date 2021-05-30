using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Helpers
{
    public static class ClaimsHelper
    {
        public static void GetAllPermissions(this List<RoleClaimResponse> allPermissions)
        {
            var fields = typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy));
            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimResponse { Value = fi.GetValue(null).ToString(), Type = ApplicationClaimTypes.Permission });
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