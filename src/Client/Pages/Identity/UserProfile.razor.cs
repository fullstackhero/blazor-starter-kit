using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;

namespace CleanBlazor.Client.Pages.Identity;

public partial class UserProfile
{
    private bool _active;
    private string _email;
    private char _firstLetterOfName;
    private string _firstName;
    private string _lastName;

    private bool _loaded;
    private string _phoneNumber;
    [Parameter] public string Id { get; set; }
    [Parameter] public string Title { get; set; }
    [Parameter] public string Description { get; set; }

    [Parameter] public string ImageDataUrl { get; set; }

    private async Task ToggleUserStatus()
    {
        var request = new ToggleUserStatusRequest { ActivateUser = _active, UserId = Id };
        await HttpClient
            .PostAsJsonAsync<ToggleUserStatusRequest, Result>(UsersEndpoints.ToggleUserStatus(request.UserId), request)
            .Match(_ =>
                {
                    SnackBar.Success(Localizer["Updated User Status."]);
                    NavigationManager.NavigateTo("/identity/users");
                },
                errors => SnackBar.Error(errors));
    }

    protected override async Task OnInitializedAsync()
    {
        var userId = Id;
        await HttpClient.GetFromJsonAsync<Result<UserResponse>>(UsersEndpoints.GetById(userId))
            .Match(async (_, user) => await InitializeUserAsync(user), _ => { });

        _loaded = true;
    }

    private async Task InitializeUserAsync(UserResponse user)
    {
        if (user != null)
        {
            _firstName = user.FirstName;
            _lastName = user.LastName;
            _email = user.Email;
            _phoneNumber = user.PhoneNumber;
            _active = user.IsActive;

            await HttpClient.GetFromJsonAsync<Result<string>>(AccountsEndpoints.GetProfilePicture(user.Id))
                .Match((_, imageData) => ImageDataUrl = imageData, _ => { });
        }

        Title = $"{_firstName} {_lastName}'s {Localizer["Profile"]}";
        Description = _email;
        if (_firstName.Length > 0)
        {
            _firstLetterOfName = _firstName[0];
        }
    }
}
