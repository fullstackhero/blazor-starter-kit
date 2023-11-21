using Blazored.LocalStorage;
using CleanBlazor.Client.Configuration;
using CleanBlazor.Shared.Constants.Storage;
using CleanBlazor.Shared.Settings;
using CleanBlazor.Shared.Wrapper;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CleanBlazor.Client;

public class ClientPreferenceManager
{
    private readonly IStringLocalizer<ClientPreferenceManager> _localizer;
    private readonly ILocalStorageService _localStorageService;

    public ClientPreferenceManager(
        ILocalStorageService localStorageService,
        IStringLocalizer<ClientPreferenceManager> localizer)
    {
        _localStorageService = localStorageService;
        _localizer = localizer;
    }

    public async Task<bool> ToggleDarkModeAsync()
    {
        if (await GetPreference() is not ClientPreference preference)
        {
            return false;
        }

        preference.IsDarkMode = !preference.IsDarkMode;
        await SetPreference(preference);
        return !preference.IsDarkMode;
    }

    public async Task<Result> ChangeLanguageAsync(string languageCode)
    {
        if (await GetPreference() is not ClientPreference preference)
        {
            return Result.Fail(_localizer["Failed to get client preferences"]);
        }

        preference.LanguageCode = languageCode;
        await SetPreference(preference);
        return Result.Ok(_localizer["Client Language has been changed"]);
    }

    public async Task<MudTheme> GetCurrentThemeAsync()
    {
        if (await GetPreference() is not ClientPreference preference)
        {
            return ApplicationTheme.DefaultTheme;
        }

        return preference.IsDarkMode ? ApplicationTheme.DarkTheme : ApplicationTheme.DefaultTheme;
    }

    public async Task<IPreference> GetPreference() =>
        await _localStorageService.GetItemAsync<ClientPreference>(StorageConstants.Local.Preference) ??
        new ClientPreference();

    public async Task SetPreference(IPreference preference) =>
        await _localStorageService.SetItemAsync(StorageConstants.Local.Preference, preference as ClientPreference);

    public async Task<bool> ToggleLayoutDirection()
    {
        if (await GetPreference() is not ClientPreference preference)
        {
            return false;
        }

        preference.IsRtl = !preference.IsRtl;
        await SetPreference(preference);
        return preference.IsRtl;

    }

    public async Task<bool> IsRtl()
    {
        var preference = await GetPreference() as ClientPreference;
        return preference is not { IsDarkMode: true } && preference.IsRtl;
    }
}
