using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class DocumentsEndpoint
    {
        public static string GetAllPaged(int pageNumber, int pageSize)
        {
            return $"api/documents?pageNumber={pageNumber}&pageSize={pageSize}";
        }
        public static string Save = "api/documents";
        public static string Delete = "api/documents";
    }
}
