using System.Security.Claims;
using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Client.Shared.Dialogs;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Constants.Storage;
using CleanBlazor.Shared.Enums;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Identity;

public partial class Profile
{
    private readonly UpdateProfileRequest _profileModel = new();

    private IBrowserFile _file;
    private char _firstLetterOfName;
    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    private string UserId { get; set; }

    [Parameter] public string ImageDataUrl { get; set; }

    private async Task UpdateProfileAsync() =>
        await HttpClient.PutAsJsonAsync<UpdateProfileRequest, Result>(AccountsEndpoints.UpdateProfile, _profileModel)
            .Match(async _ =>
                {
                    await AuthenticationManager.LogoutAsync();
                    SnackBar.Success(Localizer["Your Profile has been updated. Please Login to Continue."]);
                },
                errors => SnackBar.Error(errors));

    protected override async Task OnInitializedAsync() => await LoadDataAsync();

    private async Task LoadDataAsync()
    {
        ClaimsPrincipal user = await StateProvider.GetCurrentUserAsync();
        _profileModel.Email = user.GetEmail();
        _profileModel.FirstName = user.GetFirstName();
        _profileModel.LastName = user.GetLastName();
        _profileModel.PhoneNumber = user.GetPhoneNumber();
        UserId = user.GetUserId();
        await HttpClient.GetFromJsonAsync<Result<string>>(AccountsEndpoints.GetProfilePicture(UserId))
            .Match((_, imageData) => ImageDataUrl = imageData, _ => { });

        if (_profileModel.FirstName.Length > 0)
        {
            _firstLetterOfName = _profileModel.FirstName[0];
        }
    }

    private async Task UploadFiles(InputFileChangeEventArgs e)
    {
        _file = e.File;
        if (_file != null)
        {
            var extension = Path.GetExtension(_file.Name);
            var fileName = $"{UserId}-{Guid.NewGuid()}{extension}";
            const string format = "image/png";
            IBrowserFile imageFile = await e.File.RequestImageFileAsync(format, 400, 400);
            var buffer = new byte[imageFile.Size];
            await imageFile.OpenReadStream().ReadAsync(buffer);
            var request = new UpdateProfilePictureRequest
            {
                Data = buffer, FileName = fileName, Extension = extension, UploadType = UploadType.ProfilePicture
            };
            await HttpClient.PostAsJsonAsync<UpdateProfilePictureRequest, Result<string>>(
                    AccountsEndpoints.UpdateProfilePicture(UserId), request)
                .Match(async (_, base64Image) =>
                    {
                        await LocalStorage.SetItemAsync(StorageConstants.Local.UserImageUrl, base64Image);
                        SnackBar.Success(Localizer["Profile picture added."]);
                        NavigationManager.NavigateTo("/account", true);
                    },
                    errors => SnackBar.Error(errors));
        }
    }

    private async Task DeleteAsync()
    {
        var parameters = new DialogParameters
        {
            {
                nameof(DeleteConfirmation.ContentText),
                $"{string.Format(Localizer["Do you want to delete the profile picture of {0}"], _profileModel.Email)}?"
            }
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
            var request = new UpdateProfilePictureRequest
            {
                Data = null, FileName = string.Empty, UploadType = UploadType.ProfilePicture
            };
            await HttpClient.PostAsJsonAsync<UpdateProfilePictureRequest, Result<string>>(
                AccountsEndpoints.UpdateProfilePicture(UserId), request)
                .Match(async (_, _) =>
                    {
                        await LocalStorage.RemoveItemAsync(StorageConstants.Local.UserImageUrl);
                        ImageDataUrl = string.Empty;
                        SnackBar.Success(Localizer["Profile picture deleted."]);
                        NavigationManager.NavigateTo("/account", true);
                    },
                    errors => SnackBar.Error(errors));
        }
    }
}
