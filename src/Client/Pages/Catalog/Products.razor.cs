using System.Security.Claims;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Client.Shared.Dialogs;
using CleanBlazor.Contracts.Catalog.Products;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Catalog;

public partial class Products
{
    private bool _bordered;
    private bool _canCreateProducts;
    private bool _canDeleteProducts;
    private bool _canEditProducts;
    private bool _canExportProducts;
    private bool _canSearchProducts;
    private int _currentPage;

    private ClaimsPrincipal _currentUser;
    private bool _dense;
    private bool _loaded;

    private IEnumerable<GetAllPagedProductsResponse> _pagedData;
    private string _searchString = "";
    private bool _striped = true;
    private MudTable<GetAllPagedProductsResponse> _table;
    private int _totalItems;

    [CascadingParameter] private HubConnection HubConnection { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await StateProvider.GetCurrentUserAsync();
        _canCreateProducts = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Create))
            .Succeeded;
        _canEditProducts = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Edit))
            .Succeeded;
        _canDeleteProducts = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Delete))
            .Succeeded;
        _canExportProducts = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Export))
            .Succeeded;
        _canSearchProducts = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Products.Search))
            .Succeeded;

        _loaded = true;
        HubConnection = HubConnection.TryInitialize(NavigationManager, LocalStorage);
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
    }

    private async Task<TableData<GetAllPagedProductsResponse>> ServerReload(TableState state)
    {
        if (!string.IsNullOrWhiteSpace(_searchString))
        {
            state.Page = 0;
        }

        await LoadData(state.Page, state.PageSize, state);
        return new TableData<GetAllPagedProductsResponse> { TotalItems = _totalItems, Items = _pagedData };
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        string[] orderings = null;
        if (!string.IsNullOrEmpty(state.SortLabel))
        {
            orderings = state.SortDirection != SortDirection.None
                ? new[] { $"{state.SortLabel} {state.SortDirection}" }
                : new[] { $"{state.SortLabel}" };
        }

        var request = new GetAllPagedProductsRequest
        {
            PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString, OrderBy = orderings
        };
        var endpoint = ProductsEndpoints.GetAllPaged(
            request.PageNumber,
            request.PageSize,
            request.SearchString,
            request.OrderBy);
        await HttpClient.GetFromJsonAsync<PaginatedResult<GetAllPagedProductsResponse>>(endpoint)
            .Match((_, result) =>
                {
                    _totalItems = result.TotalCount;
                    _currentPage = result.CurrentPage;
                    _pagedData = result.Data;
                },
                errors => SnackBar.Error(errors));
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private async Task ExportToExcel()
    {
        var endpoint = string.IsNullOrWhiteSpace(_searchString)
            ? ProductsEndpoints.Export
            : ProductsEndpoints.ExportFiltered(_searchString);
        await HttpClient.GetFromJsonAsync<Result<string>>(endpoint)
            .Match(async (_, base64Data) =>
                {
                    await JsRuntime.InvokeVoidAsync("Download",
                        new
                        {
                            ByteArray = base64Data,
                            FileName = $"{nameof(Products).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                            MimeType = ApplicationConstants.MimeTypes.OpenXml
                        });
                    SnackBar.Success(string.IsNullOrWhiteSpace(_searchString)
                        ? Localizer["Products exported"]
                        : Localizer["Filtered Products exported"]);
                },
                errors => SnackBar.Error(errors));
    }

    private async Task InvokeModal(int id = 0)
    {
        var parameters = new DialogParameters();
        if (id != 0)
        {
            GetAllPagedProductsResponse product = _pagedData.FirstOrDefault(c => c.Id == id);
            if (product != null)
            {
                parameters.Add(nameof(AddEditProductModal.AddEditProductModel),
                    new AddEditProductRequest
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

        var options = new DialogOptions
        {
            CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true
        };
        IDialogReference dialog =
            await DialogService.ShowAsync<AddEditProductModal>(id == 0 ? Localizer["Create"] : Localizer["Edit"],
                parameters,
                options);
        DialogResult result = await dialog.Result;
        if (!result.Canceled)
        {
            OnSearch("");
        }
    }

    private async Task Delete(int id)
    {
        string deleteContent = Localizer["Delete Content"];
        var parameters = new DialogParameters
        {
            { nameof(DeleteConfirmation.ContentText), string.Format(deleteContent, id) }
        };
        var options = new DialogOptions
        {
            CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true
        };
        IDialogReference dialog =
            await DialogService.ShowAsync<DeleteConfirmation>(Localizer["Delete"], parameters, options);
        DialogResult dialogResult = await dialog.Result;
        if (!dialogResult.Canceled)
        {
            await HttpClient.DeleteFromJsonAsync<Result<int>>(ProductsEndpoints.DeleteById(id))
                .Match(async (message, _) =>
                    {
                        await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                        SnackBar.Success(message);
                    },
                    errors => SnackBar.Error(errors));
            OnSearch("");
        }
    }
}
