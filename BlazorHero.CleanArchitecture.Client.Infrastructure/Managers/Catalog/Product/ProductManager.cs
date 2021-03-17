using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetProductImage;
using BlazorHero.CleanArchitecture.Application.Requests.Catalog;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Catalog.Product
{
    public class ProductManager : IProductManager
    {
        private readonly HttpClient _httpClient;

        public ProductManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<GetProductImageResponse>> GetProductImageAsync(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<Result<GetProductImageResponse>>(Routes.ProductsEndpoint.GetProductImage(id));
            return response;
        }

        public async Task<PaginatedResult<GetAllPagedProductsResponse>> GetProductsAsync(GetAllPagedProductsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductsEndpoint.GetAllPaged(request.PageNumber, request.PageSize));
            return await response.ToPaginatedResult<GetAllPagedProductsResponse>();
        }
    }
}
