namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class DocumentTypesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/documentTypes/export";

        public static string GetAll = "api/documentTypes";
        public static string Delete = "api/documentTypes";
        public static string Save = "api/documentTypes";
        public static string GetCount = "api/documentTypes/count";
    }
}