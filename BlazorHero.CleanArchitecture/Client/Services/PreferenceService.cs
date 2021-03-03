using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Client.Interfaces;
using BlazorHero.CleanArchitecture.Client.Settings;
using MudBlazor;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services
{
    public class PreferenceService : IPreferenceService
    {
        private readonly ILocalStorageService _localStorageService;

        public PreferenceService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<bool> ToggleDarkModeAsync()
        {
            var preference = await GetPreference();
            preference.IsDarkMode = !preference.IsDarkMode;
            await SetPreference(preference);
            return !preference.IsDarkMode;
        }

        public async Task ChangeLanguageAsync(string languageCode)
        {
            var preference = await GetPreference();
            preference.LanguageCode = languageCode;
            await SetPreference(preference);
        }

        public async Task<MudTheme> GetCurrentThemeAsync()
        {
            var preference = await GetPreference();
            if (preference.IsDarkMode) return BlazorHeroTheme.DarkTheme;
            return BlazorHeroTheme.DefaultTheme;
        }

        public async Task<Preference> GetPreference()
        {
            return await _localStorageService.GetItemAsync<Preference>("preference") ?? new Preference();
        }

        public async Task SetPreference(Preference preference)
        {
            await _localStorageService.SetItemAsync("preference", preference);
        }
    }
}