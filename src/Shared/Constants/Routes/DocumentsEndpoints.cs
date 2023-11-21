namespace CleanBlazor.Shared.Constants.Routes;

public static class DocumentsEndpoints
{
    private const string Prefix = "api/v1/documents";

    public const string Save = Prefix;

    public static string GetAllPaged(int pageNumber, int pageSize, string searchString) =>
        $"{Prefix}?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";

    public static string GetById(int documentId) => $"{Prefix}/{documentId}";
    public static string DeleteById(int documentId) => $"{Prefix}/{documentId}";
}
