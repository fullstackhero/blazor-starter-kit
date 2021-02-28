using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Client.Extensions;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Personal
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
            await _accountService.UpdateProfiledAsync(profileModel);
        }
        protected override async Task OnInitializedAsync() => await LoadDataAsync();
        private async Task LoadDataAsync()
        {
            var state = await _authState.GetAuthenticationStateAsync();
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
        void DeletePicture()
        {
            if (!String.IsNullOrEmpty(AvatarImageLink))
            {
                AvatarImageLink = null;
                AvatarIcon = Icons.Material.Outlined.SentimentVeryDissatisfied;
                AvatarButtonText = "Upload Picture";
                AvatarButtonColor = Color.Primary;
            }
            else
            {
            }
        }
    }
}
