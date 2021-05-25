namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class AuditEndpoints
    {
        public static string DownloadFileFiltered(string searchString, bool searchInOldValues = false, bool searchInNewValues = false)
        {
            return $"{DownloadFile}?searchString={searchString}&searchInOldValues={searchInOldValues}&searchInNewValues={searchInNewValues}";
        }

        public static string GetCurrentUserTrails = "api/audits";
        public static string DownloadFile = "api/audits/export";
    }
}