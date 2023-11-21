using System.Security.Claims;
using AutoMapper;
using CleanBlazor.Client.Configuration.Mappings;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Identity;

public partial class RolePermissions
{
    private bool _bordered;
    private bool _canEditRolePermissions;
    private bool _canSearchRolePermissions;

    private ClaimsPrincipal _currentUser;
    private bool _dense;
    private bool _loaded;
    private IMapper _mapper;

    private PermissionResponse _model;
    private RoleClaimResponse _roleClaims = new();
    private string _searchString = "";
    private RoleClaimResponse _selectedItem = new();
    private bool _striped = true;

    [CascadingParameter] private HubConnection HubConnection { get; set; }
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string Description { get; set; }
    private Dictionary<string, List<RoleClaimResponse>> GroupedRoleClaims { get; } = new();

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await StateProvider.GetCurrentUserAsync();
        _canEditRolePermissions =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.RoleClaims.Edit)).Succeeded;
        _canSearchRolePermissions =
            (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.RoleClaims.Search)).Succeeded;

        await GetRolePermissionsAsync();
        _loaded = true;
        HubConnection = HubConnection.TryInitialize(NavigationManager, LocalStorage);
        if (HubConnection.State == HubConnectionState.Disconnected)
        {
            await HubConnection.StartAsync();
        }
    }

    private async Task GetRolePermissionsAsync()
    {
        _mapper = new MapperConfiguration(c => { c.AddProfile<RoleProfile>(); }).CreateMapper();
        var roleId = Id;
        await HttpClient.GetFromJsonAsync<Result<PermissionResponse>>(RolesEndpoints.GetPermissionsById(roleId))
            .Match((_, permissions) => ProcessPermissions(permissions),
                errors =>
                {
                    SnackBar.Error(errors);
                    NavigationManager.NavigateTo("/identity/roles");
                });
    }

    private void ProcessPermissions(PermissionResponse permissions)
    {
        _model = permissions;
        GroupedRoleClaims.Add(Localizer["All Permissions"], _model.RoleClaims);
        foreach (RoleClaimResponse claim in _model.RoleClaims)
        {
            if (GroupedRoleClaims.TryGetValue(claim.Group, out List<RoleClaimResponse> roleClaim))
            {
                roleClaim.Add(claim);
            }
            else
            {
                GroupedRoleClaims.Add(claim.Group, new List<RoleClaimResponse> { claim });
            }
        }

        if (_model != null)
        {
            Description = string.Format(Localizer["Manage {0} {1}'s Permissions"],
                _model.RoleId,
                _model.RoleName);
        }
    }

    private async Task SaveAsync()
    {
        PermissionRequest request = _mapper.Map<PermissionResponse, PermissionRequest>(_model);
        await HttpClient.PutAsJsonAsync<PermissionRequest, Result<string>>(
                RolesEndpoints.UpdatePermissionsId(request.RoleId), request)
            .Match(async (message, _) =>
                {
                    SnackBar.Success(message);
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.OnChangeRolePermissions,
                        _currentUser.GetUserId(),
                        request.RoleId);
                    NavigationManager.NavigateTo("/identity/roles");
                },
                errors => SnackBar.Error(errors));
    }

    private bool Search(RoleClaimResponse roleClaims)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
        {
            return true;
        }

        if (roleClaims.Value?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        if (roleClaims.Description?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        return false;
    }

    private static Color GetGroupBadgeColor(int selected, int all)
    {
        if (selected == 0)
        {
            return Color.Error;
        }

        return selected == all ? Color.Success : Color.Info;
    }
}
