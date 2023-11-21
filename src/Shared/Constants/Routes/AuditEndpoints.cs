namespace CleanBlazor.Shared.Constants.Routes;

public static class AuditEndpoints
{
    private const string Prefix = "api/v1/audits";

    public const string GetCurrentUserTrails = Prefix;
    public const string DownloadFile = $"{Prefix}/export";

    public static string DownloadFileFiltered(
        string searchString,
        bool searchInOldValues = false,
        bool searchInNewValues = false) =>
        $"{DownloadFile}?searchString={searchString}&searchInOldValues={searchInOldValues}&searchInNewValues={searchInNewValues}";
}
