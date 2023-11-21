using System.Net.Http.Json;
using System.Text.Json;

namespace CleanBlazor.Client.Extensions;

public static class HttpClientExtensions
{
    public static async Task<TResponse> PostAsJsonAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string url,
        TRequest request,
        JsonSerializerOptions options = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync(url, request, options, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(options, cancellationToken);
    }

    public static async Task<TResponse> PutAsJsonAsync<TRequest, TResponse>(
        this HttpClient httpClient,
        string url,
        TRequest request,
        JsonSerializerOptions options = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PutAsJsonAsync(url, request, options, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(options, cancellationToken);
    }

    public static async Task<TResponse> GetFromJsonAsync<TResponse>(
        this HttpClient httpClient,
        string url,
        JsonSerializerOptions options = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(options, cancellationToken);
    }

    public static async Task<TResponse> DeleteFromJsonAsync<TResponse>(
        this HttpClient httpClient,
        string url,
        JsonSerializerOptions options = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.DeleteAsync(url, cancellationToken);
        return await response.Content.ReadFromJsonAsync<TResponse>(options, cancellationToken);
    }
}
