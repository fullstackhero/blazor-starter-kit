namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class TokenEndpoints
    {
        public static string Get = "api/identity/token";
        public static string Refresh = "api/identity/token/refresh";
    }
}