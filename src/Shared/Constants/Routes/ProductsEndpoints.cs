namespace CleanBlazor.Shared.Constants.Routes;

public static class ProductsEndpoints
{
    private const string Prefix = "api/v1/products";

    public const string Save = Prefix;
    public const string Export = $"{Prefix}/export";

    public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
    {
        var url = $"{Prefix}?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
        if (orderBy?.Any() != true)
        {
            return url;
        }

        url = orderBy.Aggregate(url, (current, orderByPart) => current + $"{orderByPart},");

        url = url[..^1];

        return url;
    }

    public static string GetProductImage(int productId) => $"{Prefix}/image/{productId}";
    public static string ExportFiltered(string searchString) => $"{Export}?searchString={searchString}";
    public static string DeleteById(int productId) => $"{Prefix}/{productId}";
}
