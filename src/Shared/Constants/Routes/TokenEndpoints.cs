namespace CleanBlazor.Shared.Constants.Routes;

public static class TokenEndpoints
{
    private const string Prefix = "api/v1/identity/token";

    public const string Get = Prefix;
    public const string Refresh = $"{Prefix}/refresh";
}
