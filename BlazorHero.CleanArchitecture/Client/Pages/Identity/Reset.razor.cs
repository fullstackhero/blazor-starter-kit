using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Reset
    {
        private bool success;
        private string[] errors = { };
        private MudForm form;

        [Parameter]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Parameter]
        [Required]
        public string Password { get; set; }

        [Parameter]
        [Required]
        public string ConfirmPassword { get; set; }

        [Parameter]
        public string Token { get; set; }

        protected override void OnInitialized()
        {
            var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out var param))
            {
                var queryToken = param.First();
                Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(queryToken));
            }
        }

        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(Token))
            {
                form.Validate();
                if (form.IsValid)
                {
                    var request = new ResetPasswordRequest { Email = Email, Password = Password, Token = Token };
                    var result = await _userManager.ResetPasswordAsync(request);
                    if (result.Succeeded)
                    {
                        _snackBar.Add(localizer[result.Messages[0]], Severity.Success);
                        _navigationManager.NavigateTo("/");
                    }
                    else
                    {
                        foreach (var message in result.Messages)
                        {
                            _snackBar.Add(localizer[message], Severity.Error);
                        }
                    }
                }
            }
            else
            {
                _snackBar.Add(localizer["Token Not Found!"], Severity.Error);
            }
        }

        private bool PasswordVisibility;
        private InputType PasswordInput = InputType.Password;
        private string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility()
        {
            if (PasswordVisibility)
            {
                PasswordVisibility = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                PasswordVisibility = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

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

        private MudTextField<string> pwField;

        private string PasswordMatch(string arg)
        {
            if (pwField.Value != arg)
                return "Passwords don't match";
            return null;
        }
    }
}