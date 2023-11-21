using System.Security.Claims;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Identity;

public partial class Users
{
    private bool _bordered;
    private bool _canCreateUsers;
    private bool _canExportUsers;
    private bool _canSearchUsers;
    private bool _canViewRoles;

    private ClaimsPrincipal _currentUser;
    private bool _dense;
    private bool _loaded;
    private string _searchString = "";
    private bool _striped = true;
    private UserResponse _user = new();
    private List<UserResponse> _userList = new();

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await StateProvider.GetCurrentUserAsync();
        _canCreateUsers = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Create))
            .Succeeded;
        _canSearchUsers = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Search))
            .Succeeded;
        _canExportUsers = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Users.Export))
            .Succeeded;
        _canViewRoles = (await AuthorizationService.AuthorizeAsync(_currentUser, Permissions.Roles.View)).Succeeded;

        await GetUsersAsync();
        _loaded = true;
    }

    private async Task GetUsersAsync() =>
        await HttpClient.GetFromJsonAsync<Result<List<UserResponse>>>(UsersEndpoints.GetAll)
            .Match((_, users) => _userList = users.ToList(),
                errors => SnackBar.Error(errors));

    private bool Search(UserResponse user)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
        {
            return true;
        }

        if (user.FirstName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        if (user.LastName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        if (user.Email?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        if (user.PhoneNumber?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        if (user.UserName?.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true)
        {
            return true;
        }

        return false;
    }

    private async Task ExportToExcel()
    {
        var endpoint = string.IsNullOrWhiteSpace(_searchString)
            ? UsersEndpoints.Export
            : UsersEndpoints.ExportFiltered(_searchString);
        var base64 = await HttpClient.GetStringAsync(endpoint);
        await JsRuntime.InvokeVoidAsync("Download",
            new
            {
                ByteArray = base64,
                FileName = $"{nameof(Users).ToLower()}_{DateTime.Now:ddMMyyyyHHmmss}.xlsx",
                MimeType = ApplicationConstants.MimeTypes.OpenXml
            });
        SnackBar.Success(string.IsNullOrWhiteSpace(_searchString)
                ? Localizer["Users exported"]
                : Localizer["Filtered Users exported"]);
    }

    private async Task InvokeModal()
    {
        var parameters = new DialogParameters();
        var options = new DialogOptions
        {
            CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true
        };
        IDialogReference dialog =
            await DialogService.ShowAsync<RegisterUserModal>(Localizer["Register New User"], parameters, options);
        DialogResult result = await dialog.Result;
        if (!result.Canceled)
        {
            await GetUsersAsync();
        }
    }

    private void ViewProfile(string userId) => NavigationManager.NavigateTo($"/user-profile/{userId}");

    private void ManageRoles(string userId, string email)
    {
        if (email == "mukesh@blazorhero.com")
        {
            SnackBar.Error(Localizer["Not Allowed."]);
        }
        else
        {
            NavigationManager.NavigateTo($"/identity/user-roles/{userId}");
        }
    }
}
