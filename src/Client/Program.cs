using System.Globalization;
using CleanBlazor.Client.Configuration;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Shared.Constants.Localization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CleanBlazor.Client;

public static class Program
{
    public static async Task Main(string[] args)
    {
        WebAssemblyHostBuilder builder = WebAssemblyHostBuilder
            .CreateDefault(args)
            .AddRootComponents()
            .AddClientServices();
        WebAssemblyHost host = builder.Build();
        var storageService = host.Services.GetRequiredService<ClientPreferenceManager>();
        if (storageService != null)
        {
            CultureInfo culture;
            if (await storageService.GetPreference() is ClientPreference preference)
            {
                culture = new CultureInfo(preference.LanguageCode);
            }
            else
            {
                culture = new CultureInfo(LocalizationConstants.DefaultLanguage.Code);
            }

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        await builder.Build().RunAsync();
    }
}
