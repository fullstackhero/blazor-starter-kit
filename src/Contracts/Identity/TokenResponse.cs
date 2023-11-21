namespace CleanBlazor.Contracts.Identity;

public class TokenResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string UserImageUrl { get; set; }
    public DateTimeOffset RefreshTokenExpiryTime { get; set; }
}
