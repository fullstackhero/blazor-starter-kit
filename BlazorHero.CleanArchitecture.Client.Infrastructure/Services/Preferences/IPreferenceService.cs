using BlazorHero.CleanArchitecture.Client.Infrastructure.Settings;
using MudBlazor;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Preferences
{
    public interface IPreferenceService
    {
        Task SetPreference(Preference preference);

        Task<Preference> GetPreference();

        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();

        Task ChangeLanguageAsync(string languageCode);
    }
}