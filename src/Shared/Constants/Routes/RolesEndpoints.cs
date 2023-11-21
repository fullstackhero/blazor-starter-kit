namespace CleanBlazor.Shared.Constants.Routes;

public static class RolesEndpoints
{
    private const string Prefix = "api/v1/identity/roles";

    public const string GetAll = Prefix;
    public const string Save = Prefix;

    public static string GetPermissionsById(string roleId) => $"{Prefix}/{roleId}/permissions";
    public static string UpdatePermissionsId(string roleId) => $"{Prefix}/{roleId}/permissions";
    public static string DeleteById(string roleId) => $"{Prefix}/{roleId}";
}
