namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class ExtendedAttributesEndpoints
    {
        public static string GetAll(string entityName) => $"api/{entityName}ExtendedAttributes";
        public static string GetAllByEntityId<TEntityId>(string entityName, TEntityId entityId) => $"{GetAll(entityName)}/by-entity/{entityId}";
        public static string Export(string entityName) => $"api/{entityName}ExtendedAttributes/export";
        public static string ExportFiltered(string entityName, string searchString, bool includeEntity) => $"{Export(entityName)}?searchString={searchString}&{nameof(includeEntity)}={includeEntity}";
        public static string Delete(string entityName) => $"api/{entityName}ExtendedAttributes";
        public static string Save(string entityName) => $"api/{entityName}ExtendedAttributes";
        public static string GetCount(string entityName) => $"api/{entityName}ExtendedAttributes/count";
    }
}