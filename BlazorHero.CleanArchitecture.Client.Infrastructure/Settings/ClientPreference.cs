using BlazorHero.CleanArchitecture.Shared.Settings;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Settings
{
    public record ClientPreference : IPreference
    {
        public bool IsDarkMode { get; set; }
        public bool IsRTL { get; set; }
        public bool IsDrawerOpen { get; set; }
        public string PrimaryColor { get; set; }
        public string LanguageCode { get; set; } = "en-US";
    }
}