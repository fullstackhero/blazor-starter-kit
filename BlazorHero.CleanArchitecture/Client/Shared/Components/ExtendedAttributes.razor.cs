using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.AddEdit;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId;
using BlazorHero.CleanArchitecture.Application.Mappings;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.ExtendedAttribute;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace BlazorHero.CleanArchitecture.Client.Shared.Components
{
    public class ExtendedAttributesLocalization
    {
        // for localization
    }

    public abstract partial class ExtendedAttributes<TId, TEntityId, TEntity, TExtendedAttribute>
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>
        where TId : IEquatable<TId>
    {
        [Inject] private IExtendedAttributeManager<TId, TEntityId, TEntity, TExtendedAttribute> ExtendedAttributeManager { get; set; }

        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public string EntityIdString { get; set; }
        [Parameter] public string EntityName { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }

        public abstract TEntityId EntityId { get; }
        public abstract string ExtendedAttributesEditPolicyName { get; }
        public abstract string ExtendedAttributesCreatePolicyName { get; }
        public abstract string ExtendedAttributesDeletePolicyName { get; }
        public abstract string ExtendedAttributesExportPolicyName { get; }

        protected string CurrentUserId { get; set; }
        protected List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>> _model;
        protected Dictionary<string, List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>> GroupedExtendedAttributes { get; } = new();
        protected IMapper _mapper;
        protected GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId> _extendedAttributes = new();
        protected GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId> _selectedItem = new();
        protected string _searchString = "";
        protected bool _dense = true;
        protected bool _striped = true;
        protected bool _bordered = false;

        private ClaimsPrincipal _currentUser;
        protected bool _canEditExtendedAttributes;
        protected bool _canCreateExtendedAttributes;
        protected bool _canDeleteExtendedAttributes;
        protected bool _canExportExtendedAttributes;

        protected override async Task OnInitializedAsync()
        {
            base.OnInitializedAsync();

            _currentUser = await _authenticationManager.CurrentUser();
            _canEditExtendedAttributes = _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesEditPolicyName).Result.Succeeded;
            _canCreateExtendedAttributes = _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesCreatePolicyName).Result.Succeeded;
            _canDeleteExtendedAttributes = _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesDeletePolicyName).Result.Succeeded;
            _canExportExtendedAttributes = _authorizationService.AuthorizeAsync(_currentUser, ExtendedAttributesExportPolicyName).Result.Succeeded;

            _mapper = new MapperConfiguration(c => { c.AddProfile<ExtendedAttributeProfile>(); }).CreateMapper();
            await GetExtendedAttributesAsync();

            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                CurrentUserId = user.GetUserId();
            }

            HubConnection = HubConnection.TryInitialize(_navigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        protected async Task GetExtendedAttributesAsync()
        {
            var response = await ExtendedAttributeManager.GetAllByEntityIdAsync(EntityId);
            if (response.Succeeded)
            {
                GroupedExtendedAttributes.Clear();
                _model = response.Data;
                GroupedExtendedAttributes.Add(_localizer["All Groups"], _model);
                foreach (var extendedAttribute in _model)
                {
                    if (!string.IsNullOrWhiteSpace(extendedAttribute.ExternalId))
                    {
                        if (GroupedExtendedAttributes.ContainsKey(extendedAttribute.ExternalId))
                        {
                            GroupedExtendedAttributes[extendedAttribute.ExternalId].Add(extendedAttribute);
                        }
                        else
                        {
                            GroupedExtendedAttributes.Add(extendedAttribute.ExternalId, new List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>> { extendedAttribute });
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
                _navigationManager.NavigateTo("/document-store");
            }
        }

        protected async Task OnSearch(string text)
        {
            _searchString = text;
            await GetExtendedAttributesAsync();
        }

        protected async Task InvokeModal(TId id = default)
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

        protected async Task Delete(TId id)
        {
            string deleteContent = _localizer["Delete Content"];
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
                     await  OnSearch("");
                    _snackBar.Add(response.Messages[0], Severity.Success);
                }
                else
                {                 
                    await OnSearch("");
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
        }

        protected async Task Reset()
        {
            _model = new List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>();
            await GetExtendedAttributesAsync();
        }

        protected Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByExternalId = response => response.ExternalId;
        protected Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByType = response => response.Type;
        protected Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByDescription = response => response.Description;
        protected Func<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>, object> SortByIsActive = response => response.IsActive;

        protected bool Search(GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId> extendedAttributes)
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
            if (extendedAttributes.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
            {
                return true;
            }
            return false;
        }

        protected Color GetGroupBadgeColor(int selected, int all)
        {
            if (selected == 0)
                return Color.Error;

            if (selected == all)
                return Color.Success;

            return Color.Info;
        }
    }
}