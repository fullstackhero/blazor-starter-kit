using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Constants.Permission;

namespace BlazorHero.CleanArchitecture.DataAccess.MsSql.Helpers
{
    public static class ClaimsHelper
    {
        public static async Task GeneratePermissionClaimByModule(this RoleManager<IdentityRole> roleManager, IdentityRole role, string module)
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

        public static async Task AddCustomPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
            }
        }
    }
}