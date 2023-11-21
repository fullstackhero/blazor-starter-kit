namespace CleanBlazor.Shared.Constants.Localization;

public static class LocalizationConstants
{
    public static readonly LanguageCode[] SupportedLanguages =
    {
        new() { Code = "en-US", DisplayName = "English" },
        new() { Code = "fr-FR", DisplayName = "French" }
    };

    public static LanguageCode DefaultLanguage => SupportedLanguages.First(x => x.Code == "en-US");
}
