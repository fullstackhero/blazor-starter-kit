namespace CleanBlazor.Contracts.Identity;

public class RefreshTokenRequest
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
