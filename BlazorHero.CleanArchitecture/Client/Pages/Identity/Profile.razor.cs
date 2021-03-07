using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Client.Extensions;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Profile
    {
        private char FirstLetterOfName { get; set; }

        public string AvatarImageLink { get; set; } = string.Empty;
        public string AvatarIcon { get; set; }
        public string AvatarButtonText { get; set; } = "Delete Picture";
        public Color AvatarButtonColor { get; set; } = Color.Error;
        public IEnumerable<string> Errors { get; set; }
        private readonly UpdateProfileRequest profileModel = new UpdateProfileRequest();

        private async Task UpdateProfileAsync()
        {
            var response = await _accountManager.UpdateProfileAsync(profileModel);
            if (response.Succeeded)
            {
                await _authenticationManager.Logout();
                _snackBar.Add("Your Profile has been updated. Please Login to Continue.", Severity.Success);
                _navigationManager.NavigateTo("/");
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
            }
        }

        protected override async Task OnInitializedAsync() => await LoadDataAsync();

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            profileModel.Email = user.GetEmail();
            profileModel.FirstName = user.GetFirstName();
            profileModel.LastName = user.GetLastName();
            profileModel.PhoneNumber = user.GetPhoneNumber();
            if (profileModel.FirstName.Length > 0)
            {
                FirstLetterOfName = profileModel.FirstName[0];
            }
        }
    }
}