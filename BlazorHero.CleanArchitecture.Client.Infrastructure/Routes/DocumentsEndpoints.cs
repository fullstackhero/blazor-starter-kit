namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class DocumentsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/documents?pageNumber={pageNumber}&pageSize={pageSize}";
        }

        public static string Save = "api/documents";
        public static string Delete = "api/documents";
    }
}