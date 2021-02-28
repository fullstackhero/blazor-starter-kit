using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Personal
{
    public partial class Security
    {
        private readonly ChangePasswordRequest passwordModel = new ChangePasswordRequest();
        protected override async Task OnInitializedAsync()
        {
        }
        private async Task ChangePasswordAsync()
        {
            await _accountService.ChangePasswordAsync(passwordModel);
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
        MudTextField<string> pwField;

        private string PasswordMatch(string arg)
        {
            if (pwField.Value != arg)
                return "Passwords don't match";
            return null;
        }
    }
}
