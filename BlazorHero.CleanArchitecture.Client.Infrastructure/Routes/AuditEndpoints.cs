namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class AuditEndpoints
    {
        public static string GetCurrentUserTrails = "api/audits";
        public static string DownloadFile = "api/audits/export";
    }
}