namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class RolesEndpoints
    {
        public static string Delete = "api/identity/role";
        public static string GetAll = "api/identity/role";
        public static string Save = "api/identity/role";
        public static string GetPermissions = "api/identity/role/permissions/";
        public static string UpdatePermissions = "api/identity/role/permissions/update";
    }
}