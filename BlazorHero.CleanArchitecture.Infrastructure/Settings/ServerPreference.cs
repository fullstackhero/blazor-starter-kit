using BlazorHero.CleanArchitecture.Shared.Settings;

namespace BlazorHero.CleanArchitecture.Infrastructure.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = "en-US";
    }
}
