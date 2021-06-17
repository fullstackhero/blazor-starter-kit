namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class ProductsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            string url = $"api/v1/products?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (!(orderBy == null || orderBy.Length == 0))
            {
                foreach (string OrderByPart in orderBy)
                {
                    url = url + OrderByPart + ",";
                }
                url = url.Substring(0, url.Length - 1); // loose training ,
            }
            return url;
        }

        public static string GetCount = "api/v1/products/count";

        public static string GetProductImage(int productId)
        {
            return $"api/v1/products/image/{productId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Save = "api/v1/products";
        public static string Delete = "api/v1/products";
        public static string Export = "api/v1/products/export";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";
    }
}