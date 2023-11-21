namespace CleanBlazor.Shared.Constants.Routes;

public static class DocumentTypesEndpoints
{
    private const string Prefix = "api/v1/documentTypes";

    public const string GetAll = Prefix;
    public const string Save = Prefix;
    public const string Export = $"{Prefix}/export";

    public static string ExportFiltered(string searchString) => $"{Export}?searchString={searchString}";
    public static string DeleteById(int documentTypeId) => $"{Prefix}/{documentTypeId}";
}
