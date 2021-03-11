using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Application.Requests.Catalog;
using BlazorHero.CleanArchitecture.Domain.Entities.Catalog;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class Products
    {
        private List<GetAllPagedProductsResponse> productList;

        protected override async Task OnInitializedAsync()
        {
            var request = new GetAllPagedProductsRequest { PageSize = 10, PageNumber = 1 };
            var response = await _productManager.GetProductsAsync(request);
            productList = response.Data;
        }
        private IEnumerable<GetAllPagedProductsResponse> pagedData;
        private MudTable<GetAllPagedProductsResponse> table;

        private int totalItems;
        private int currentPage;
        private string searchString = null;
        private async Task<TableData<GetAllPagedProductsResponse>> ServerReload(TableState state)
        {
            var request = new GetAllPagedProductsRequest { PageSize = state.PageSize, PageNumber = state.Page };
            var response = await _productManager.GetProductsAsync(request);
            IEnumerable<GetAllPagedProductsResponse> data = response.Data;
            data = data.Where(element =>
            {
                if (string.IsNullOrWhiteSpace(searchString))
                    return true;
                if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (element.Barcode.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                return false;
            }).ToArray();
            totalItems = response.TotalCount;
            switch (state.SortLabel)
            {
                case "nr_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Id);
                    break;
                case "sign_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Name);
                    break;
                case "name_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Description);
                    break;
                case "position_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Barcode);
                    break;
                case "mass_field":
                    data = data.OrderByDirection(state.SortDirection, o => o.Rate);
                    break;
            }
            currentPage = response.Page;
            pagedData = await ReloadTable(state.Page, state.PageSize);
            return new TableData<GetAllPagedProductsResponse>() { TotalItems = totalItems, Items = pagedData };
        }
        async Task<IEnumerable<GetAllPagedProductsResponse>> ReloadTable(int pageNumber, int pageSize)
        {
            var request = new GetAllPagedProductsRequest { PageSize = pageSize, PageNumber = pageNumber + 1 };
            var response = await  _productManager.GetProductsAsync(request);
            return response.Data;
        }
        private void OnSearch(string text)
        {
            searchString = text;
            table.ReloadServerData();
        }
    }
}
