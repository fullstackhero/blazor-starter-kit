using System.Linq;
using BlazorHero.CleanArchitecture.Application.Constants.Localization;
using BlazorHero.CleanArchitecture.Infrastructure.Interfaces.Settings;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Settings
{
    public record ClientPreference : IPreference
    {
        public bool IsDarkMode { get; set; }
        public bool IsRTL { get; set; }
        public bool IsDrawerOpen { get; set; }
        public string PrimaryColor { get; set; }
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US";
    }
}