using System.Security.Claims;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Client.Shared.Dialogs;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Misc;

public partial class DocumentTypes
{
    private bool _bordered;
    private bool _canCreateDocumentTypes;
    private bool _canDeleteDocumentTypes;
    private bool _canEditDocumentTypes;
    private bool _canExportDocumentTypes;
    private bool _canSearchDocumentTypes;

    private ClaimsPrincipal _currentUser;
    private bool _dense;
    private GetAllDocumentTypesResponse _documentType = new();

    private List<GetAllDocumentTypesResponse> _documentTypeList = new();
    private bool _loaded;
    private string _searchString = "";
    private bool _striped = true;

    [CascadingParameter] private HubConnection HubConnection { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await StateProvider.GetCurrentUserAsync();
        _canCreateDocumentTypes =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.DocumentTypes.Create)).Succeeded;
        _canEditDocumentTypes =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.DocumentTypes.Edit)).Succeeded;
        _canDeleteDocumentTypes =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.DocumentTypes.Delete)).Succeeded;
        _canExportDocumentTypes =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.DocumentTypes.Export)).Succeeded;
        _canSearchDocumentTypes =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.DocumentTypes.Search)).Succeeded;

        await GetDocumentTypesAsync();
        _loaded = true;
        HubConnection = HubConnection.TryInitialize(NavigationManager, LocalStorage);
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
    }

    private async Task GetDocumentTypesAsync() =>
        await HttpClient.GetFromJsonAsync<Result<List<GetAllDocumentTypesResponse>>>(DocumentTypesEndpoints.GetAll)
            .Match((_, documentTypes) => _documentTypeList = documentTypes.ToList(),
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
        IDialogReference dialog =
            await DialogService.ShowAsync<DeleteConfirmation>(Localizer["Delete"], parameters, options);
        DialogResult dialogResult = await dialog.Result;
        if (!dialogResult.Canceled)
        {
            await HttpClient.DeleteFromJsonAsync<Result<int>>(DocumentTypesEndpoints.DeleteById(id))
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
            ? DocumentTypesEndpoints.Export
            : DocumentTypesEndpoints.ExportFiltered(_searchString);
        await HttpClient.GetFromJsonAsync<Result<string>>(endpoint)
            .Match(async (_, base64Data) =>
                {
                    await JsRuntime.InvokeVoidAsync("Download",
                        new
                        {
                            ByteArray = base64Data,
                            FileName = $"{nameof(DocumentTypes).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                            MimeType = ApplicationConstants.MimeTypes.OpenXml
                        });
                    SnackBar.Success(string.IsNullOrWhiteSpace(_searchString)
                        ? Localizer["Document Types exported"]
                        : Localizer["Filtered Document Types exported"]);
                },
                errors => SnackBar.Error(errors));
    }

    private async Task InvokeModal(int id = 0)
    {
        var parameters = new DialogParameters();
        if (id != 0)
        {
            _documentType = _documentTypeList.FirstOrDefault(c => c.Id == id);
            if (_documentType != null)
            {
                parameters.Add(nameof(AddEditDocumentTypeModal.AddEditDocumentTypeModel),
                    new AddEditDocumentTypeRequest
                    {
                        Id = _documentType.Id, Name = _documentType.Name, Description = _documentType.Description
                    });
            }
        }

        var options = new DialogOptions
        {
            CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true
        };
        IDialogReference dialog =
            await DialogService.ShowAsync<AddEditDocumentTypeModal>(id == 0 ? Localizer["Create"] : Localizer["Edit"],
                parameters,
                options);
        DialogResult result = await dialog.Result;
        if (!result.Canceled)
        {
            await Reset();
        }
    }

    private async Task Reset()
    {
        _documentType = new GetAllDocumentTypesResponse();
        await GetDocumentTypesAsync();
    }

    private bool Search(GetAllDocumentTypesResponse brand)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
        {
            return true;
        }

        if (brand.Name?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        if (brand.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        return false;
    }
}
