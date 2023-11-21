namespace CleanBlazor.Shared.Constants.Routes;

public static class AccountsEndpoints
{
    private const string Prefix = "api/v1/identity/accounts";

    public const string Register = $"{Prefix}/register";
    public const string ChangePassword = $"{Prefix}/change-password";
    public const string UpdateProfile = $"{Prefix}/update-profile";

    public static string GetProfilePicture(string userId) => $"{Prefix}/{userId}/profile-picture";

    public static string UpdateProfilePicture(string userId) => $"{Prefix}/{userId}/profile-picture";
}
