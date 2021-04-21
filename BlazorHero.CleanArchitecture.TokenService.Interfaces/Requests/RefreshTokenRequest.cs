namespace BlazorHero.CleanArchitecture.TokenService.Interfaces.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}