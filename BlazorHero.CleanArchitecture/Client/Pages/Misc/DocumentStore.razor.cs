using BlazorHero.CleanArchitecture.Application.Features.Documents.Queries.GetAll;
using BlazorHero.CleanArchitecture.Application.Requests.Documents;
using BlazorHero.CleanArchitecture.Client.Extensions;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Documents.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;

namespace BlazorHero.CleanArchitecture.Client.Pages.Misc
{
    public partial class DocumentStore
    {
        private IEnumerable<GetAllDocumentsResponse> _pagedData;
        private MudTable<GetAllDocumentsResponse> _table;
        private string CurrentUserId { get; set; }
        private int _totalItems;
        private int _currentPage;
        private string _searchString = "";
        private bool _dense = true;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canCreateDocuments;
        private bool _canEditDocuments;
        private bool _canDeleteDocuments;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canCreateDocuments = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Documents.Create)).Succeeded;
            _canEditDocuments = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Documents.Edit)).Succeeded;
            _canDeleteDocuments = (await _authorizationService.AuthorizeAsync(_currentUser, Permissions.Documents.Delete)).Succeeded;

            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                CurrentUserId = user.GetUserId();
            }
        }

        private async Task<TableData<GetAllDocumentsResponse>> ServerReload(TableState state)
        {
            await LoadData(state.Page, state.PageSize, state);
            return new TableData<GetAllDocumentsResponse> { TotalItems = _totalItems, Items = _pagedData };
        }

        private async Task LoadData(int pageNumber, int pageSize, TableState state)
        {
            var request = new GetAllPagedDocumentsRequest { PageSize = pageSize, PageNumber = pageNumber + 1 };
            var response = await _documentManager.GetAllAsync(request);
            if (response.Succeeded)
            {
                _totalItems = response.TotalCount;
                _currentPage = response.CurrentPage;
                var data = response.Data;
                var loadedData = data.Where(element =>
                {
                    if (string.IsNullOrWhiteSpace(_searchString))
                        return true;
                    if (element.Title.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (element.Description.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                });
                switch (state.SortLabel)
                {
                    case "documentIdField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, d => d.Id);
                        break;
                    case "documentTitleField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, d => d.Title);
                        break;
                    case "documentDescriptionField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, d => d.Description);
                        break;
                    case "documentIsPublicField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, d => d.IsPublic);
                        break;
                    case "documentDateCreatedField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, d => d.CreatedOn);
                        break;
                    case "documentOwnerField":
                        loadedData = loadedData.OrderByDirection(state.SortDirection, d => d.CreatedBy);
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

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                var doc = _pagedData.FirstOrDefault(c => c.Id == id);
                if (doc != null)
                {
                    parameters.Add(nameof(AddEditDocumentModal.AddEditDocumentModel), new AddEditDocumentCommand
                    {
                        Id = doc.Id,
                        Title = doc.Title,
                        Description = doc.Description,
                        URL = doc.URL,
                        IsPublic = doc.IsPublic
                    });
                }
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditDocumentModal>(id == 0 ? _localizer["Create"] : _localizer["Edit"], parameters, options);
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
                var response = await _documentManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                    OnSearch("");
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