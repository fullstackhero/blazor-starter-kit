using System;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.CurrentUserService.Interfaces;
using BlazorHero.CleanArchitecture.CustomLocalStorageService.Interfaces;
using BlazorHero.CleanArchitecture.Infrastructure.Interfaces.Settings;
using BlazorHero.CleanArchitecture.Infrastructure.Settings;

namespace BlazorHero.CleanArchitecture.Infrastructure.Managers.Preferences
{
    //TODO - implement

    public class ServerPreferenceManager : IServerPreferenceManager
    {
        //private readonly ICustomLocalStorageService _localStorageService;
        //private readonly ICurrentUserService _currentUserService;

        public ServerPreferenceManager(
            //ICustomLocalStorageService localStorageService,
            //ICurrentUserService currentUserService
            )
        {
            //_localStorageService = localStorageService;
            //_currentUserService = currentUserService;
        }

        public async Task<IPreference> GetPreference()
        {
            throw new NotImplementedException();
            //return await _localStorageService.GetItemAsync<ServerPreference>("serverPreference") ?? new ServerPreference(_currentUserService);
        }

        public async Task SetPreference(IPreference preference)
        {
            throw new NotImplementedException();
            //await _localStorageService.SetItemAsync("serverPreference", preference as ServerPreference);
        }
    }
}
