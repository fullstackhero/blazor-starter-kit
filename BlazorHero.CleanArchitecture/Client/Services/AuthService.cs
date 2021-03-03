using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using BlazorHero.CleanArchitecture.Client.Authentication;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            this._httpClient = httpClient;
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        //public async Task<Result> Register(RegisterRequest model)
        //    => await this.httpClient
        //        .PostAsJsonAsync(Routes.AuthenticationEndpoint.Register, model)
        //        .ToResult();

        public async Task<IResult> Login(TokenRequest model)
        {
            var response = await this._httpClient.PostAsJsonAsync(Routes.TokenEndpoint.Get, model);
            if (!response.IsSuccessStatusCode)
            {
                var errors = await response.Content.ReadFromJsonAsync<string[]>();
                return Result.Fail(errors.ToString());
            }
            var responseAsString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<Result<TokenResponse>>(responseAsString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var token = responseObject.Data.Token;
            await localStorage.SetItemAsync("authToken", token);
            ((BlazorHeroStateProvider)this.authenticationStateProvider).MarkUserAsAuthenticated(model.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return Result.Success();
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("authToken");

            ((BlazorHeroStateProvider)authenticationStateProvider).MarkUserAsLoggedOut();

            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task ResetToken(string newToken, string email)
        {
            await localStorage.RemoveItemAsync("authToken");
            await localStorage.SetItemAsync("authToken", newToken);
            ((BlazorHeroStateProvider)this.authenticationStateProvider).MarkUserAsAuthenticated(email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newToken);
        }
    }
}