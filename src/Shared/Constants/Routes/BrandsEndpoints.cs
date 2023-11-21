namespace CleanBlazor.Shared.Constants.Routes;

public static class BrandsEndpoints
{
    private const string Prefix = "api/v1/brands";

    public const string GetAll = Prefix;
    public const string Save = Prefix;
    public const string Import = $"{Prefix}/import";
    public const string Export = $"{Prefix}/export";

    public static string ExportFiltered(string searchString) => $"{Export}?searchString={searchString}";
    public static string DeleteById(int id) => $"{Prefix}/{id}";
}
