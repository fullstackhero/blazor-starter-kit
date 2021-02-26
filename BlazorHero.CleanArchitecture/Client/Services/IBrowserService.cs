using BlazorHero.CleanArchitecture.Client.Settings;
using MudBlazor;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services
{
    public interface IBrowserService
    {
        Task SetPreference(Preference preference);
        Task<Preference> GetPreference();
        Task<MudTheme> GetCurrentThemeAsync();
        Task<bool> ToggleDarkMode();
    }
}
