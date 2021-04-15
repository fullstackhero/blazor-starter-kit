using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Security
    {
        [Inject] private Microsoft.Extensions.Localization.IStringLocalizer<Security> localizer { get; set; }

        private readonly ChangePasswordRequest passwordModel = new ChangePasswordRequest();

        private async Task ChangePasswordAsync()
        {
            var response = await _accountManager.ChangePasswordAsync(passwordModel);
            if (response.Succeeded)
            {
                _snackBar.Add(localizer["Password Changed!"], Severity.Success);
                passwordModel.Password = string.Empty;
                passwordModel.NewPassword = string.Empty;
                passwordModel.ConfirmNewPassword = string.Empty;
            }
            else
            {
                foreach (var error in response.Messages)
                {
                    _snackBar.Add(localizer[error], Severity.Error);
                }
            }
        }

        private IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return localizer["Password is required!"];
                yield break;
            }
            if (pw.Length < 8)
                yield return localizer["Password must be at least of length 8"];
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return localizer["Password must contain at least one capital letter"];
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return localizer["Password must contain at least one lowercase letter"];
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return localizer["Password must contain at least one digit"];
        }

        private MudTextField<string> pwField;

        private string PasswordMatch(string arg)
        {
            if (pwField.Value != arg)
                return localizer["Passwords don't match"];
            return null;
        }
    }
}