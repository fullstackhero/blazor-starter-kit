using BlazorHero.CleanArchitecture.Client.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
           => await WebAssemblyHostBuilder
               .CreateDefault(args)
               .AddRootComponents()
               .AddClientServices()
               .Build()
               .RunAsync();
    }
}