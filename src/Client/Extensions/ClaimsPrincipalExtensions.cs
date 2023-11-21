using System.Security.Claims;

namespace CleanBlazor.Client.Extensions;

internal static class ClaimsPrincipalExtensions
{
    internal static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Email);

    internal static string GetFirstName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Name);

    internal static string GetLastName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.Surname);

    internal static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.MobilePhone);

    internal static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

    internal static bool IsWithinExpirationThreshold(
        this ClaimsPrincipal claimsPrincipal,
        DateTimeOffset nowUtc,
        int thresholdInMinutes = 1)
        => ExpiresIn(claimsPrincipal, nowUtc).TotalMinutes <= thresholdInMinutes;

    private static TimeSpan ExpiresIn(this ClaimsPrincipal claimsPrincipal, DateTimeOffset nowUtc) =>
        GetExpirationTime(claimsPrincipal) - nowUtc;

    private static DateTimeOffset GetExpirationTime(this ClaimsPrincipal claimsPrincipal)
    {
        var exp = claimsPrincipal.FindFirstValue("exp");
        return DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
    }

}
