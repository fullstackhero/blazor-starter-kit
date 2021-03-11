namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class UserEndpoint
    {
        public static string GetAll = "api/identity/user";
        public static string Get(string userId)
        {
            return $"api/identity/user/{userId}";
        }
        public static string Register = "api/identity/user";
        public static string ToggleUserStatus = "api/identity/user/toggle-status";
    }
}