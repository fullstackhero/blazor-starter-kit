using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Infrastructure.Settings;
using BlazorHero.CleanArchitecture.Shared.Settings;

namespace BlazorHero.CleanArchitecture.Infrastructure.Managers.Preferences
{
    //public class ServerPreferenceManager : IServerPreferenceManager
    //{
    //    private readonly ILocalStorageService _localStorageService;
    //    private readonly ICurrentUserService _currentUserService;

    //    public ServerPreferenceManager(
    //        ILocalStorageService localStorageService,
    //        ICurrentUserService currentUserService)
    //    {
    //        _localStorageService = localStorageService;
    //        _currentUserService = currentUserService;
    //    }

    //    public async Task<IPreference> GetPreference()
    //    {
    //        return await _localStorageService.GetItemAsync<ServerPreference>("serverPreference") ?? new ServerPreference(_currentUserService);
    //    }

    //    public async Task SetPreference(IPreference preference)
    //    {
    //        await _localStorageService.SetItemAsync("serverPreference", preference as ServerPreference);
    //    }
    //}
}
