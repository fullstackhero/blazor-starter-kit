using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Constants.Storage;
using Microsoft.AspNetCore.Components.Authorization;

namespace CleanBlazor.Client.Authentication;

public class ApplicationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public ApplicationStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<ClaimsPrincipal> GetCurrentUserAsync()
    {
        AuthenticationState state = await GetAuthenticationStateAsync();
        return state.User;
    }

    public async Task StateChangedAsync()
    {
        Task<AuthenticationState> authState = Task.FromResult(await GetAuthenticationStateAsync());

        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(anonymousUser));

        NotifyAuthenticationStateChanged(authState);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var savedToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);
        if (string.IsNullOrWhiteSpace(savedToken))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var state = new AuthenticationState(
            new ClaimsPrincipal(new ClaimsIdentity(GetClaimsFromJwt(savedToken), "jwt")));
        return state;
    }

    private static IEnumerable<Claim> GetClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs == null)
        {
            return claims;
        }

        keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

        if (roles != null)
        {
            if (roles.ToString().Trim().StartsWith('['))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                claims.AddRange(parsedRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        keyValuePairs.TryGetValue(ApplicationClaimTypes.Permission, out var permissions);
        if (permissions != null)
        {
            if (permissions.ToString().Trim().StartsWith('['))
            {
                var parsedPermissions = JsonSerializer.Deserialize<string[]>(permissions.ToString());
                claims.AddRange(parsedPermissions.Select(permission =>
                    new Claim(ApplicationClaimTypes.Permission, permission)));
            }
            else
            {
                claims.Add(new Claim(ApplicationClaimTypes.Permission, permissions.ToString()));
            }

            keyValuePairs.Remove(ApplicationClaimTypes.Permission);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string payload)
    {
        payload = payload.Trim().Replace('-', '+').Replace('_', '/');
        var base64 = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
        return Convert.FromBase64String(base64);
    }
}
