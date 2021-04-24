using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Preferences;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Settings;
using BlazorHero.CleanArchitecture.Shared.Constants.Localization;

namespace BlazorHero.CleanArchitecture.Client
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder
                          .CreateDefault(args)
                          .AddRootComponents()
                          .AddClientServices();
            var host = builder.Build();
            var storageService = host.Services.GetRequiredService<ClientPreferenceManager>();
            if (storageService != null)
            {
                CultureInfo culture;
                var preference = await storageService.GetPreference() as ClientPreference;
                if (preference != null)
                    culture = new CultureInfo(preference.LanguageCode);
                else
                    culture = new CultureInfo(LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US");
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
            await builder.Build().RunAsync();
        }
    }
}