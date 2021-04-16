using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Infrastructure.Settings;
using BlazorHero.CleanArchitecture.Shared.Settings;

namespace BlazorHero.CleanArchitecture.Infrastructure.Managers.Preferences
{
    public class ServerPreferenceManager : IServerPreferenceManager
    {
        private readonly ILocalStorageService _localStorageService;

        public ServerPreferenceManager(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public async Task ChangeLanguageAsync(string languageCode)
        {
            var preference = await GetPreference() as ServerPreference;
            if (preference != null)
            {
                preference.LanguageCode = languageCode;
                await SetPreference(preference);
            }
        }

        public async Task<IPreference> GetPreference()
        {
            return await _localStorageService.GetItemAsync<ServerPreference>("serverPreference") ?? new ServerPreference();
        }

        public async Task SetPreference(IPreference preference)
        {
            await _localStorageService.SetItemAsync("serverPreference", preference);
        }
    }
}
