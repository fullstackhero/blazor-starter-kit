using BlazorHero.CleanArchitecture.Application.Features.Products.Queries.GetAllPaged;
using BlazorHero.CleanArchitecture.Application.Requests.Catalog;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Products.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace BlazorHero.CleanArchitecture.Client.Pages.Catalog
{
    public partial class Products
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private IEnumerable<GetAllPagedProductsResponse> _pagedData;
        private MudTable<GetAllPagedProductsResponse> _table;
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateProducts;
        private bool _canEditProducts;
        private bool _canDeleteProducts;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create)).Succeeded;
            _canEditProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit)).Succeeded;
            _canDeleteProducts = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete)).Succeeded;

            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task<TableData<GetAllPagedProductsResponse>> ServerReload(TableState state)
        {
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllPagedProductsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            var request = new GetAllPagedProductsRequest { PageSize = pageSize, PageNumber = pageNumber + 1 };
            var response = await _productManager.GetProductsAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                var data = response.Data;
                var loadedData = data.Where(element =>
                {
                    if (string.IsNullOrWhiteSpace(_searchString))
                        return true;
                    if (element.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                        return true;
                    if (element.Brand?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                        return true;
                    if (element.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                        return true;
                    if (element.Barcode?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
                        return true;
                    return false;
                });
                switch (state.SortLabel)
                {
                    case "productIdField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, p => p.Id);
                        break;
                    case "productNameField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, p => p.Name);
                        break;
                    case "productBrandField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, p => p.Brand);
                        break;
                    case "productDescriptionField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, p => p.Description);
                        break;
                    case "productBarcodeField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, p => p.Barcode);
                        break;
                    case "productRateField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, p => p.Rate);
                        break;
                }
                data = loadedData.ToList();
                _pagedData = data;
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private void OnSearch(string text)
        {
            _searchString = text;
            _table.ReloadServerData();
        }

        private async Task ExportToExcel()
        {
            var base64 = await _productManager.ExportToExcelAsync(_searchString);
            await _jsRuntime.InvokeVoidAsync("Download", new
            {
                ByteArray = base64,
                FileName = $"{nameof(Products).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            });
            _snackBar.Add(string.IsNullOrWhiteSpace(_searchString)
                ? _localizer["Products exported"]
                : _localizer["Filtered Products exported"], Severity.Success);
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var product = _pagedData.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    parameters.Add(nameof(AddEditProductModal.AddEditProductModel), new AddEditProductCommand
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Rate = product.Rate,
                        BrandId = product.BrandId,
                        Barcode = product.Barcode
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditProductModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                OnSearch("");
            }
        }

        private async Task Delete(int id)
        {
            string deleteContent = _localizer["Delete Content"];
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Shared.Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await _productManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }
    }
}