namespace BlazorHero.CleanArchitecture.Application.Configurations
{
    public class AppConfiguration
    {
        public string Secret { get; set; }

        public bool BehindSSLProxy { get; set; }

        public string ProxyIP { get; set; }

        public string ApplicationUrl { get; set; }
    }
}