namespace CleanBlazor.Shared.Constants.Routes;

public static class RoleClaimsEndpoints
{
    private const string Prefix = "api/v1/identity/roleClaims";

    public const string GetAll = Prefix;
    public const string Save = Prefix;

    public static string GetAllByRoleId(string roleId) => $"{Prefix}/{roleId}";
    public static string DeleteByRoleId(string roleId) => $"{Prefix}/{roleId}";
}
