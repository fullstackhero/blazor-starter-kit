namespace BlazorHero.CleanArchitecture.UserService.Interfaces.Requests
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}