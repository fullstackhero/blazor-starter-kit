using CleanBlazor.Shared.Constants.Localization;
using CleanBlazor.Shared.Settings;

namespace CleanBlazor.Server.Configuration;

public record ServerPreference : IPreference
{
    public string LanguageCode { get; set; } = LocalizationConstants.DefaultLanguage.Code;

    //TODO - add server preferences
}
