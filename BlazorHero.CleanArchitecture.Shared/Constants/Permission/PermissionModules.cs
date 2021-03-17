using System.Collections.Generic;

namespace BlazorHero.CleanArchitecture.Shared.Constants.Permission
{
    public static class PermissionModules
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
        }

        public static List<string> GetAllPermissionsModules()
        {
            return new List<string>()
            {
                Users,
                Roles,
                Products,
                Brands
            };
        }

        public const string Users = "Users";
        public const string Roles = "Roles";
        public const string Products = "Products";
        public const string Brands = "Brands";
    }
}