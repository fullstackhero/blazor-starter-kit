using System.Security.Claims;
using Blazored.LocalStorage;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Constants.Storage;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace CleanBlazor.Client.Authentication;

public class AuthenticationManager
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly NavigationManager _navigationManager;
    private readonly TimeProvider _timeProvider;

    public AuthenticationManager(
        IHttpClientFactory httpClientFactory,
        ILocalStorageService localStorage,
        AuthenticationStateProvider authenticationStateProvider,
        NavigationManager navigationManager,
        TimeProvider timeProvider)
    {
        _httpClientFactory = httpClientFactory;
        _localStorage = localStorage;
        _authenticationStateProvider = authenticationStateProvider;
        _navigationManager = navigationManager;
        _timeProvider = timeProvider;
    }

    public ValueTask<string> GetAccessTokenAsync() =>
        _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);

    public async Task LogoutAsync()
    {
        // TODO: revoke refresh token server-side
        await _localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
        await _localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
        await _localStorage.RemoveItemAsync(StorageConstants.Local.UserImageUrl);
        ((ApplicationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _navigationManager.NavigateTo("/", forceLoad: true);
    }

    public async Task<Result> LoginAsync(TokenRequest model)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.HttpClient.ClientName);
        var result = await httpClient.PostAsJsonAsync<TokenRequest, Result<TokenResponse>>(TokenEndpoints.Get, model);
        if (result.IsFailure)
        {
            return Result.Fail(result.ErrorMessages);
        }

        await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, result.Data.Token);
        await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, result.Data.RefreshToken);
        await _localStorage.SetItemAsync(StorageConstants.Local.UserImageUrl, result.Data.UserImageUrl);

        await ((ApplicationStateProvider)_authenticationStateProvider).StateChangedAsync();

        return Result.Ok();
    }

    public async Task<Result<string>> RefreshAsync()
    {
        var request = new RefreshTokenRequest
        {
            Token = await _localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken),
            RefreshToken = await _localStorage.GetItemAsync<string>(StorageConstants.Local.RefreshToken)
        };

        if (string.IsNullOrEmpty(request.RefreshToken))
        {
            return Result.Fail<string>("No refresh token found. Please log in.");
        }

        ClaimsPrincipal user = await ((ApplicationStateProvider)_authenticationStateProvider).GetCurrentUserAsync();
        if (!user.IsWithinExpirationThreshold(_timeProvider.GetUtcNow()))
        {
            // Token is still valid, no need to refresh
            return request.Token;
        }

        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.HttpClient.ClientName);
        var result = await httpClient.PostAsJsonAsync<RefreshTokenRequest, Result<TokenResponse>>(
            TokenEndpoints.Refresh, request);

        if (result.IsFailure)
        {
            await LogoutAsync();
            return Result.Fail<string>(result.ErrorMessages);
        }

        await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, result.Data.Token);
        await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, result.Data.RefreshToken);

        await ((ApplicationStateProvider)_authenticationStateProvider).StateChangedAsync();

        return result.Data.Token;
    }
}
