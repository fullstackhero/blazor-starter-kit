using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CleanBlazor.Client.Authentication;

public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly AuthenticationManager _authenticationManager;
    private readonly ILogger<AuthenticationDelegatingHandler> _logger;

    private bool _refreshing;

    public AuthenticationDelegatingHandler(
        AuthenticationManager authenticationManager,
        ILogger<AuthenticationDelegatingHandler> logger)
    {
        _authenticationManager = authenticationManager;
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // TODO: Check if the request is to our own server
        // Attempt to attach an access token to the request if available
        await AttachAccessTokenAsync(request);

        // Send the initial request
        var response = await base.SendAsync(request, cancellationToken);

        // If not unauthorized, or we are already refreshing the token, or bearer header is not set
        // return the original response
        if (response.StatusCode != HttpStatusCode.Unauthorized
            || _refreshing
            || string.IsNullOrEmpty(request.Headers.Authorization?.Parameter))
        {
            return response;
        }

        // Try to refresh the token
        return await RefreshTokenAndRetryAsync(request, response, cancellationToken);
    }

    private async Task AttachAccessTokenAsync(HttpRequestMessage request)
    {
        var token = await _authenticationManager.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task<HttpResponseMessage> RefreshTokenAndRetryAsync(
        HttpRequestMessage request,
        HttpResponseMessage originalResponse,
        CancellationToken cancellationToken)
    {
        _refreshing = true;

        try
        {
            var result = await _authenticationManager.RefreshAsync();
            if (result.IsFailure)
            {
                _logger.LogError("Error occurred when attempting to refresh token. {Errors}", result.ErrorMessages);
                return new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = JsonContent.Create(result)
                };
            }

            // If refresh is successful, attach the new token and retry the request
            if (!string.IsNullOrEmpty(result.Data))
            {
                _logger.LogInformation("Token refreshed");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.Data);
                return await base.SendAsync(request, cancellationToken);
            }
        }
        finally
        {
            _refreshing = false;
        }

        // If we've reached here, token refresh has failed; return the original response
        return originalResponse;
    }
}
