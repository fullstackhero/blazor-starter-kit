using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Infrastructure.Models.Identity;
using CleanBlazor.Shared.Constants.Permission;
using Microsoft.AspNetCore.Identity;

namespace CleanBlazor.Infrastructure.Helpers;

public static class ClaimsHelper
{
    public static void GetAllPermissions(this List<RoleClaimResponse> allPermissions)
    {
        Type[] modules = typeof(Permissions).GetNestedTypes();

        foreach (Type module in modules)
        {
            var moduleName = string.Empty;
            var moduleDescription = string.Empty;

            if (module.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .FirstOrDefault() is DisplayNameAttribute displayNameAttribute)
            {
                moduleName = displayNameAttribute.DisplayName;
            }

            if (module.GetCustomAttributes(typeof(DescriptionAttribute), true)
                    .FirstOrDefault() is DescriptionAttribute descriptionAttribute)
            {
                moduleDescription = descriptionAttribute.Description;
            }

            FieldInfo[] fields =
                module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (FieldInfo fi in fields)
            {
                var propertyValue = fi.GetValue(null);

                if (propertyValue is not null)
                {
                    allPermissions.Add(new RoleClaimResponse
                    {
                        Value = propertyValue.ToString(),
                        Type = ApplicationClaimTypes.Permission,
                        Group = moduleName,
                        Description = moduleDescription
                    });
                }
            }
        }
    }

    public static async Task<IdentityResult> AddPermissionClaim(
        this RoleManager<ApplicationRole> roleManager,
        ApplicationRole role,
        string permission)
    {
        IList<Claim> allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == ApplicationClaimTypes.Permission && a.Value == permission))
        {
            return await roleManager.AddClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, permission));
        }

        return IdentityResult.Failed();
    }
}
