using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Helpers
{
    public static class ClaimsHelper
    {
        public static ReadOnlyCollection<RoleClaimResponse> AllPermissions;
        static ClaimsHelper()
        {
            List<RoleClaimResponse> allPermissions = new List<RoleClaimResponse>();
            IEnumerable<object> permissionClasses = typeof(Permissions).GetNestedTypes(BindingFlags.Static | BindingFlags.Public).Cast<TypeInfo>();
            foreach (TypeInfo permissionClass in permissionClasses)
            {
                IEnumerable<FieldInfo> permissions = permissionClass.DeclaredFields.Where(f => f.IsLiteral);
                foreach (FieldInfo permission in permissions)
                {
                    RoleClaimResponse applicationPermission = new RoleClaimResponse
                    {
                        Value = permission.GetValue(null).ToString(),
                        Type = ApplicationClaimTypes.Permission,
                        Group =permissionClass.Name
                    };
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])permission.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                    {
                        applicationPermission.Description = attributes[0].Description;
                    }
                    else
                    {
                        applicationPermission.Description = permission.GetValue(null).ToString().Replace('.', ' ');
                    }
                    allPermissions.Add(applicationPermission);
                }
                AllPermissions = allPermissions.AsReadOnly();
            }
        }
        public static string[] GetAllPermissionValues()
        {
            return AllPermissions.Select(p => p.Value).ToArray();
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