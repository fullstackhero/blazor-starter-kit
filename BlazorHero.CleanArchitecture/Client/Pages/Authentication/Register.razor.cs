using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Authentication
{
    public partial class Register
    {
        private async Task SubmitAsync()
        {
            //return Task.CompletedTask;
        }
        private RegisterUserModel model { get; set; } = new RegisterUserModel();
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

        private class RegisterUserModel
        {
            [Parameter] [Required] [MinLength(6)] public string UserName { get; set; }

            [Parameter] [Required] public string FirstName { get; set; }

            [Parameter] [Required] public string LastName { get; set; }

            [Parameter] [Required] [EmailAddress] public string Email { get; set; }

            [Parameter] [Required] public string Password { get; set; }

            [Parameter] [Required] public string ConfirmPassword { get; set; }

            [Parameter] public string PhoneNumber { get; set; }
        }
    }
}
