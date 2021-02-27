using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Client.Settings;
using MudBlazor;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services
{
    public class BrowserService : IBrowserService
    {
        private readonly ILocalStorageService _localStorageService;

        public BrowserService(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task<bool> ToggleDarkMode()
        {
            var preference = await GetPreference();
            if (preference.IsDarkMode)
            {
                preference.IsDarkMode = false;
            }
            else
            {
                preference.IsDarkMode = true;
            }
            await SetPreference(preference);
            return !preference.IsDarkMode;
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