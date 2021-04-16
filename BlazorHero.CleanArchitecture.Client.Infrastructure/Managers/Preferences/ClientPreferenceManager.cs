using System.Net.Http;
using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Settings;
using MudBlazor;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Shared.Settings;
using System.Net.Http.Json;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Preferences
{
    public class ClientPreferenceManager : IClientPreferenceManager
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;

        public ClientPreferenceManager(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        public async Task<bool> ToggleDarkModeAsync()
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                preference.IsDarkMode = !preference.IsDarkMode;
                await SetPreference(preference);
                return !preference.IsDarkMode;
            }

            return false;
        }

        public async Task ChangeLanguageAsync(string languageCode)
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                preference.LanguageCode = languageCode;
                await SetPreference(preference);
            }

            // change language on the server side
            await _httpClient.PostAsJsonAsync(Routes.PreferencesEndpoint.ChangeLanguage, languageCode);
        }

        public async Task<MudTheme> GetCurrentThemeAsync()
        {
            var preference = await GetPreference() as ClientPreference;
            if (preference != null)
            {
                if (preference.IsDarkMode == true) return BlazorHeroTheme.DarkTheme;
            }
            return BlazorHeroTheme.DefaultTheme;
        }

        public async Task<IPreference> GetPreference()
        {
            return await _localStorageService.GetItemAsync<ClientPreference>("clientPreference") ?? new ClientPreference();
        }

        public async Task SetPreference(IPreference preference)
        {
            await _localStorageService.SetItemAsync("clientPreference", preference);
        }
    }
}