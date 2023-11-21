using CleanBlazor.Application.Abstractions.Infrastructure.Services.Storage;
using CleanBlazor.Server.Configuration;
using CleanBlazor.Shared.Constants.Storage;
using CleanBlazor.Shared.Settings;
using CleanBlazor.Shared.Wrapper;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Server;

public class ServerPreferenceManager
{
    private readonly IStringLocalizer<ServerPreferenceManager> _localizer;
    private readonly IServerStorageService _serverStorageService;

    public ServerPreferenceManager(
        IServerStorageService serverStorageService,
        IStringLocalizer<ServerPreferenceManager> localizer)
    {
        _serverStorageService = serverStorageService;
        _localizer = localizer;
    }

    public async Task<Result> ChangeLanguageAsync(string languageCode)
    {
        if (await GetPreference() is not ServerPreference preference)
        {
            return Result.Fail(_localizer["Failed to get server preferences"]);
        }

        preference.LanguageCode = languageCode;
        await SetPreference(preference);
        return Result.Ok(_localizer["Server Language has been changed"]);
    }

    public async Task<IPreference> GetPreference() =>
        await _serverStorageService.GetItemAsync<ServerPreference>(StorageConstants.Server.Preference) ??
        new ServerPreference();

    public async Task SetPreference(IPreference preference) =>
        await _serverStorageService.SetItemAsync(StorageConstants.Server.Preference, preference as ServerPreference);
}
