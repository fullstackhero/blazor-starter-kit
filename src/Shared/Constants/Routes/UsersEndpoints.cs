namespace CleanBlazor.Shared.Constants.Routes;

public static class UsersEndpoints
{
    private const string Prefix = "api/v1/identity/users";

    public const string GetAll = Prefix;
    public const string Register = Prefix;
    public const string Export = $"{Prefix}/export";
    public const string ForgotPassword = $"{Prefix}/forgot-password";
    public const string ResetPassword = $"{Prefix}/reset-password";

    public static string GetById(string userId) => $"{Prefix}/{userId}";
    public static string GetUserRolesById(string userId) => $"{Prefix}/{userId}/roles";
    public static string ToggleUserStatus(string userId) => $"{Prefix}/{userId}/toggle-status";
    public static string ExportFiltered(string searchString) => $"{Export}?searchString={searchString}";
}
