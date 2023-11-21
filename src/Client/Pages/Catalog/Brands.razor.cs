using System.Security.Claims;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Client.Shared.Components;
using CleanBlazor.Client.Shared.Dialogs;
using CleanBlazor.Contracts;
using CleanBlazor.Contracts.Catalog.Brands;
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

public partial class Brands
{
    private bool _bordered;
    private GetAllBrandsResponse _brand = new();

    private List<GetAllBrandsResponse> _brandList = new();
    private bool _canCreateBrands;
    private bool _canDeleteBrands;
    private bool _canEditBrands;
    private bool _canExportBrands;
    private bool _canImportBrands;
    private bool _canSearchBrands;

    private ClaimsPrincipal _currentUser;
    private bool _dense;
    private bool _loaded;
    private string _searchString = "";
    private bool _striped = true;

    [CascadingParameter] private HubConnection HubConnection { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await StateProvider.GetCurrentUserAsync();
        _canCreateBrands = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Brands.Create))
            .Succeeded;
        _canEditBrands = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Brands.Edit)).Succeeded;
        _canDeleteBrands = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Brands.Delete))
            .Succeeded;
        _canExportBrands = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Brands.Export))
            .Succeeded;
        _canSearchBrands = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Brands.Search))
            .Succeeded;
        _canImportBrands = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Brands.Import))
            .Succeeded;

        await GetBrandsAsync();
        _loaded = true;

        HubConnection = HubConnection.TryInitialize(NavigationManager, LocalStorage);
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
    }

    private async Task GetBrandsAsync() =>
        await HttpClient.GetFromJsonAsync<Result<List<GetAllBrandsResponse>>>(BrandsEndpoints.GetAll)
            .Match((_, brands) => _brandList = brands.ToList(),
                errors => SnackBar.Error(errors));

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
        IDialogReference dialog = await DialogService.ShowAsync<DeleteConfirmation>(Localizer["Delete"], parameters, options);
        DialogResult dialogResult = await dialog.Result;
        if (!dialogResult.Canceled)
        {
            await HttpClient.DeleteFromJsonAsync<Result<int>>(BrandsEndpoints.DeleteById(id))
                .Match(async (message, _) =>
                    {
                        await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                        SnackBar.Success(message);
                    },
                    errors => SnackBar.Error(errors));

            await Reset();
        }
    }

    private async Task ExportToExcel()
    {
        var endpoint = string.IsNullOrWhiteSpace(_searchString)
            ? BrandsEndpoints.Export
            : BrandsEndpoints.ExportFiltered(_searchString);
        await HttpClient.GetFromJsonAsync<Result<string>>(endpoint)
            .Match(async (_, base64data) =>
                {
                    await JsRuntime.InvokeVoidAsync("Download",
                        new
                        {
                            ByteArray = base64data,
                            FileName = $"{nameof(Brands).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                            MimeType = ApplicationConstants.MimeTypes.OpenXml
                        });
                    SnackBar.Success(string.IsNullOrWhiteSpace(_searchString)
                        ? Localizer["Brands exported"]
                        : Localizer["Filtered Brands exported"]);
                },
                errors => SnackBar.Error(errors));
    }

    private async Task InvokeModal(int id = 0)
    {
        var parameters = new DialogParameters();
        if (id != 0)
        {
            _brand = _brandList.FirstOrDefault(c => c.Id == id);
            if (_brand != null)
            {
                parameters.Add(nameof(AddEditBrandModal.AddEditBrandRequestModel),
                    new AddEditBrandRequest
                    {
                        Id = _brand.Id, Name = _brand.Name, Description = _brand.Description, Tax = _brand.Tax
                    });
            }
        }

        var options = new DialogOptions
        {
            CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true
        };
        IDialogReference dialog =
            await DialogService.ShowAsync<AddEditBrandModal>(id == 0 ? Localizer["Create"] : Localizer["Edit"],
                parameters,
                options);
        DialogResult result = await dialog.Result;
        if (!result.Canceled)
        {
            await Reset();
        }
    }

    private async Task<Result<int>> ImportExcel(UploadRequest uploadFile) =>
        await HttpClient.PostAsJsonAsync<UploadRequest, Result<int>>(BrandsEndpoints.Import, uploadFile);

    private async Task InvokeImportModal()
    {
        var parameters = new DialogParameters
        {
            { nameof(ImportExcelModal.ModelName), Localizer["Brands"].ToString() }
        };
        Func<UploadRequest, Task<Result<int>>> importExcel = ImportExcel;
        parameters.Add(nameof(ImportExcelModal.OnSaved), importExcel);
        var options = new DialogOptions
        {
            CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true
        };
        IDialogReference dialog = await DialogService.ShowAsync<ImportExcelModal>(Localizer["Import"], parameters, options);
        DialogResult result = await dialog.Result;
        if (!result.Canceled)
        {
            await Reset();
        }
    }

    private async Task Reset()
    {
        _brand = new GetAllBrandsResponse();
        await GetBrandsAsync();
    }

    private bool Search(GetAllBrandsResponse brand)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
        {
            return true;
        }

        if (brand.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        return brand.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
    }
}
