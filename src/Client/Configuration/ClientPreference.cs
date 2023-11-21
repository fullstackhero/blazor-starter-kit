using CleanBlazor.Shared.Constants.Localization;
using CleanBlazor.Shared.Settings;

namespace CleanBlazor.Client.Configuration;

public record ClientPreference : IPreference
{
    public bool IsDarkMode { get; set; }
    public bool IsRtl { get; set; }
    public bool IsDrawerOpen { get; set; }
    public string PrimaryColor { get; set; }

    public string LanguageCode { get; set; } = LocalizationConstants.DefaultLanguage.Code;
}
