using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.ExtendedAttribute;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Domain.Enums;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;

namespace BlazorHero.CleanArchitecture.Client.Shared.Components
{
    public class ExtendedAttributesLocalization
    {
        // for localization
    }

    public abstract partial class ExtendedAttributesBase<TId, TEntityId, TEntity, TExtendedAttribute>
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>
    {
        [Inject] private IExtendedAttributeManager<TId, TEntityId, TEntity, TExtendedAttribute> ExtendedAttributeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public string EntityIdString { get; set; }
        [Parameter] public string EntityName { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }

        protected abstract Func<string, TEntityId> FromStringToEntityIdTypeConverter { get; }
        protected abstract string ExtendedAttributesViewPolicyName { get; }
        protected abstract string ExtendedAttributesEditPolicyName { get; }
        protected abstract string ExtendedAttributesCreatePolicyName { get; }
        protected abstract string ExtendedAttributesDeletePolicyName { get; }
        protected abstract string ExtendedAttributesExportPolicyName { get; }
        protected abstract string ExtendedAttributesSearchPolicyName { get; }
        protected abstract RenderFragment Inherited();

        private TEntityId EntityId => FromStringToEntityIdTypeConverter.Invoke(EntityIdString);
        private string CurrentUserId { get; set; }
        private List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>> _model;
        private Dictionary<string, List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>> GroupedExtendedAttributes { get; } = new();
        private GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId> _extendedAttributes = new();
        private GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId> _selectedItem = new();
        private string _searchString = "";
        private bool _includeEntity;
        private bool _onlyCurrentGroup;
        private int _activeGroupIndex;
        private MudTabs _mudTabs;
        private bool _dense = false;
        private bool _striped = true;
        private bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        private bool _canViewExtendedAttributes;
        private bool _canEditExtendedAttributes;
        private bool _canCreateExtendedAttributes;
        private bool _canDeleteExtendedAttributes;
        private bool _canExportExtendedAttributes;
        private bool _canSearchExtendedAttributes;
        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            _currentUser = await _authenticationManager.CurrentUser();
            _canViewExtendedAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesViewPolicyName)).Succeeded;
            if (!_canViewExtendedAttributes)
            {
                _snackBar.Add(_localizer["Not Allowed."], Severity.Error);
                _navigationManager.NavigateTo("/");
            }
            _canEditExtendedAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesEditPolicyName)).Succeeded;
            _canCreateExtendedAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesCreatePolicyName)).Succeeded;
            _canDeleteExtendedAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesDeletePolicyName)).Succeeded;
            _canExportExtendedAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesExportPolicyName)).Succeeded;
            _canSearchExtendedAttributes = (await _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesSearchPolicyName)).Succeeded;

            await GetExtendedAttributesAsync();
            _loaded = true;

            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                CurrentUserId = user.GetUserId();
            }

            HubConnection = HubConnection.TryInitialize(_navigationManager, _localStorage);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetExtendedAttributesAsync()
        {
            var response = await ExtendedAttributeManager.GetAllByEntityIdAsync(EntityId);
            if (response.Succeeded)
            {
                GroupedExtendedAttributes.Clear();
                _model = response.Data;
                GroupedExtendedAttributes.Add(_localizer["All Groups"], _model);
                foreach (var extendedAttribute in _model)
                {
                    if (!string.IsNullOrWhiteSpace(extendedAttribute.Group))
                    {
                        if (GroupedExtendedAttributes.ContainsKey(extendedAttribute.Group))
                        {
                            GroupedExtendedAttributes[extendedAttribute.Group].Add(extendedAttribute);
                        }
                        else
                        {
                            GroupedExtendedAttributes.Add(extendedAttribute.Group, new List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>> { extendedAttribute });
                        }
                    }
                }

                if (_model != null)
                {
                    Description = string.Format(_localizer["Manage {0} {1}'s Extended Attributes"], EntityId, EntityName);
                }
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
                _navigationManager.NavigateTo("/");
            }
        }

        private async Task ExportToExcel()
        {
            var request = new ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>
            {
                SearchString = _searchString,
                EntityId = EntityId,
                IncludeEntity = _includeEntity,
                OnlyCurrentGroup = _onlyCurrentGroup && _activeGroupIndex != 0,
                CurrentGroup = _mudTabs.Panels[_activeGroupIndex].Text
            };
            var response = await ExtendedAttributeManager.ExportToExcelAsync(request);
            if (response.Succeeded)
            {
                await _jsRuntime.InvokeVoidAsync("Download", new
                {
                    ByteArray = response.Data,
                    FileName = $"{typeof(TExtendedAttribute).Name.ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                    MimeType = ApplicationConstants.MimeTypes.OpenXml
                });
                _snackBar.Add(string.IsNullOrWhiteSpace(request.SearchString) && !request.IncludeEntity && !request.OnlyCurrentGroup
                    ? _localizer["Extended Attributes exported"]
                    : _localizer["Filtered Extended Attributes exported"], Severity.Success);
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        private async Task InvokeModal(TId id = default)
        {
            var parameters = new DialogParameters();
            if (!id.Equals(default))
            {
                var documentExtendedAttribute = _model.FirstOrDefault(c => c.Id.Equals(id));
                if (documentExtendedAttribute != null)
                {
                    parameters.Add(nameof(AddEditExtendedAttributeModal<TId, TEntityId, TEntity, TExtendedAttribute>.AddEditExtendedAttributeModel), new AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>
                    {
                        Id = documentExtendedAttribute.Id,
                        EntityId = documentExtendedAttribute.EntityId,
                        Type = documentExtendedAttribute.Type,
                        Key = documentExtendedAttribute.Key,
                        Text = documentExtendedAttribute.Text,
                        Decimal = documentExtendedAttribute.Decimal,
                        DateTime = documentExtendedAttribute.DateTime,
                        Json = documentExtendedAttribute.Json,
                        ExternalId = documentExtendedAttribute.ExternalId,
                        Group = documentExtendedAttribute.Group,
                        Description = documentExtendedAttribute.Description,
                        IsActive = documentExtendedAttribute.IsActive
                    });
                }
            }
            else
            {
                parameters.Add(nameof(AddEditExtendedAttributeModal<TId, TEntityId, TEntity, TExtendedAttribute>.AddEditExtendedAttributeModel), new AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>
                {
                    EntityId = EntityId,
                    Type = EntityExtendedAttributeType.Text
                });
            }
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Medium, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<AddEditExtendedAttributeModal<TId, TEntityId, TEntity, TExtendedAttribute>>(id.Equals(default) ? _localizer["Create"] : _localizer["Edit"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                await Reset();
            }
        }

        private async Task Delete(TId id)
        {
            string deleteContent = _localizer["Delete Extended Attribute?"];
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, id)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = _dialogService.Show<Dialogs.DeleteConfirmation>(_localizer["Delete"], parameters, options);
            var result = await dialog.Result;
            if (!result.Cancelled)
            {
                var response = await ExtendedAttributeManager.DeleteAsync(id);
                if (response.Succeeded)
                {
                     await Reset();
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {
                    await Reset();
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        private async Task Reset()
        {
            _model = new List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>();
            _searchString = "";
            await GetExtendedAttributesAsync();
        }

        private Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortById = response => response.Id;
        private Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByType = response => response.Type;
        private Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByKey = response => response.Key;
        private Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByValue = response => response.Type switch
        {
            EntityExtendedAttributeType.Decimal => response.Decimal,
            EntityExtendedAttributeType.Text => response.Text,
            EntityExtendedAttributeType.DateTime => response.DateTime,
            EntityExtendedAttributeType.Json => response.Json,
            _ => response.Text
        };
        private Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByExternalId = response => response.ExternalId;
        private Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByGroup = response => response.Group;
        private Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByDescription = response => response.Description;
        private Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByIsActive = response => response.IsActive;

        private bool Search(GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId> extendedAttributes)
        {
            if (string.IsNullOrWhiteSpace(_searchString)) return true;
            if (extendedAttributes.Key.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (extendedAttributes.Text?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (extendedAttributes.Decimal?.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (extendedAttributes.DateTime?.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (extendedAttributes.Json?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (extendedAttributes.ExternalId?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (extendedAttributes.Group?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            if (extendedAttributes.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        private Color GetGroupBadgeColor(int selected, int all)
        {
            if (selected == 0)
                return Color.Error;

            if (selected == all)
                return Color.Success;

            return Color.Info;
        }
    }
}