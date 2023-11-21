using System.Security.Claims;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Client.Shared.Dialogs;
using CleanBlazor.Contracts.Documents;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Misc;

public partial class DocumentStore
{
    private bool _bordered;
    private bool _canCreateDocuments;
    private bool _canDeleteDocuments;
    private bool _canEditDocuments;
    private bool _canSearchDocuments;
    private int _currentPage;

    private ClaimsPrincipal _currentUser;
    private bool _dense;
    private bool _loaded;

    private IEnumerable<GetAllDocumentsResponse> _pagedData;
    private string _searchString = "";
    private bool _striped = true;
    private MudTable<GetAllDocumentsResponse> _table;
    private int _totalItems;
    private string CurrentUserId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await StateProvider.GetCurrentUserAsync();
        _canCreateDocuments = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Documents.Create))
            .Succeeded;
        _canEditDocuments = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Documents.Edit))
            .Succeeded;
        _canDeleteDocuments = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Documents.Delete))
            .Succeeded;
        _canSearchDocuments = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Documents.Search))
            .Succeeded;

        _loaded = true;

        ClaimsPrincipal user = await StateProvider.GetCurrentUserAsync();
        if (user == null)
        {
            return;
        }

        if (user.Identity?.IsAuthenticated == true)
        {
            CurrentUserId = user.GetUserId();
        }
    }

    private async Task<TableData<GetAllDocumentsResponse>> ServerReload(TableState state)
    {
        if (!string.IsNullOrWhiteSpace(_searchString))
        {
            state.Page = 0;
        }

        await LoadData(state.Page, state.PageSize, state);
        return new TableData<GetAllDocumentsResponse> { TotalItems = _totalItems, Items = _pagedData };
    }

    private async Task LoadData(int pageNumber, int pageSize, TableState state)
    {
        var request = new GetAllPagedDocumentsRequest
        {
            PageSize = pageSize, PageNumber = pageNumber + 1, SearchString = _searchString
        };
        await HttpClient.GetFromJsonAsync<PaginatedResult<GetAllDocumentsResponse>>(
            DocumentsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString))
            .Match((_, result) =>
                {
                    _totalItems = result.TotalCount;
                    _currentPage = result.CurrentPage;

                    var filteredDocuments = FilterDocuments(result.Data);
                    var sortedDocuments = SortDocuments(filteredDocuments, state);
                    _pagedData = sortedDocuments.ToList();
                },
                errors => SnackBar.Error(errors));
    }

    private IEnumerable<GetAllDocumentsResponse> FilterDocuments(IEnumerable<GetAllDocumentsResponse> documents)
    {
        return documents.Where(document =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            return document.Title.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                   document.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                   document.DocumentType.Contains(_searchString, StringComparison.OrdinalIgnoreCase);
        });
    }

    private static IEnumerable<GetAllDocumentsResponse> SortDocuments(
        IEnumerable<GetAllDocumentsResponse> documents,
        TableState state)
    {
        return state.SortLabel switch
        {
            "documentIdField" => documents.OrderByDirection(state.SortDirection, d => d.Id),
            "documentTitleField" => documents.OrderByDirection(state.SortDirection, d => d.Title),
            "documentDescriptionField" => documents.OrderByDirection(state.SortDirection, d => d.Description),
            "documentDocumentTypeField" => documents.OrderByDirection(state.SortDirection, p => p.DocumentType),
            "documentIsPublicField" => documents.OrderByDirection(state.SortDirection, d => d.IsPublic),
            "documentDateCreatedField" => documents.OrderByDirection(state.SortDirection, d => d.CreatedOn),
            "documentOwnerField" => documents.OrderByDirection(state.SortDirection, d => d.CreatedBy),
            _ => documents
        };
    }

    private void OnSearch(string text)
    {
        _searchString = text;
        _table.ReloadServerData();
    }

    private async Task InvokeModal(int id = 0)
    {
        var parameters = new DialogParameters();
        if (id != 0)
        {
            GetAllDocumentsResponse doc = _pagedData.FirstOrDefault(c => c.Id == id);
            if (doc != null)
            {
                parameters.Add(nameof(AddEditDocumentModal.AddEditDocumentModel),
                    new AddEditDocumentRequest
                    {
                        Id = doc.Id,
                        Title = doc.Title,
                        Description = doc.Description,
                        Url = doc.Url,
                        IsPublic = doc.IsPublic,
                        DocumentTypeId = doc.DocumentTypeId
                    });
            }
        }

        var options = new DialogOptions
        {
            CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true
        };
        IDialogReference dialog =
            await DialogService.ShowAsync<AddEditDocumentModal>(id == 0 ? Localizer["Create"] : Localizer["Edit"],
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
            await HttpClient.DeleteFromJsonAsync<Result<int>>(DocumentsEndpoints.DeleteById(id))
                .Match((message, _) => SnackBar.Success(message),
                    errors => SnackBar.Error(errors));
            OnSearch("");
        }
    }
}
