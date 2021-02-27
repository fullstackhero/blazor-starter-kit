using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Client.Extensions;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Personal
{
    public partial class Account
    {
        public Account()
        {

        }
        private string FirstName { get; set; }
        private string LastName { get; set; }
        private string Email { get; set; }
        private string PhoneNumber { get; set; }
        private char FirstLetterOfName { get; set; }
        public string AvatarImageLink { get; set; } = "https://media-exp1.licdn.com/dms/image/C4D03AQGNO7uV7fRi7Q/profile-displayphoto-shrink_200_200/0/1531753989819?e=1614816000&v=beta&t=t2IEQlTyem3aFB1sQXFHrDGt0yMsNkPu7jDmMPoEbLg";
        public string AvatarIcon { get; set; }
        public string AvatarButtonText { get; set; } = "Delete Picture";
        public Color AvatarButtonColor { get; set; } = Color.Error;
        public IEnumerable<string> Errors { get; set; }

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
                return;
            }
        }

        void SaveChanges(string message, Severity severity)
        {
            _snackBar.Add(message, severity, config =>
            {
                config.ShowCloseIcon = false;
            });
        }
        private readonly ChangePasswordRequest model = new ChangePasswordRequest();
        private async Task ChangePasswordAsync()
        {
            var response = await _httpClient.PutAsJsonAsync("api/identity/changepassword", model);

            if (response.IsSuccessStatusCode)
            {
                model.Password = null;
                model.NewPassword = null;
                model.ConfirmNewPassword = null;
                await _authService.Logout();
                _snackBar.Add("Your password has been changed successfully.\n Please login.",Severity.Success);
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                Errors = await response.Content.ReadFromJsonAsync<string[]>();
            }
        }
        async Task UpdateProfileAsync()
        {
            
        }

        MudForm form;
        MudTextField<string> pwField1;

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        private string PasswordMatch(string arg)
        {
            if (pwField1.Value != arg)
                return "Passwords don't match";
            return null;
        }
        protected override async Task OnInitializedAsync() => await this.LoadDataAsync();
        private async Task LoadDataAsync()
        {
            var state = await _authState.GetAuthenticationStateAsync();
            var user = state.User;

            Email = user.GetEmail();
            FirstName = user.GetFirstName();
            LastName = user.GetLastName();
            PhoneNumber = user.GetPhoneNumber();
            if (FirstName.Length > 0)
            {
                FirstLetterOfName = FirstName[0];
            }
        }
    }
}
