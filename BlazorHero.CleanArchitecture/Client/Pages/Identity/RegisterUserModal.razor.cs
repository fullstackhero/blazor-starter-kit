using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class RegisterUserModal
    {
        bool success;
        string[] errors = { };
        MudForm form;
        [Parameter]
        [Required]
        public string FirstName { get; set; }
        [Parameter]
        [Required]
        public string LastName { get; set; }
        [Parameter]
        [Required]
        public string Email { get; set; }
        [Parameter]
        [Required]
        public string Password { get; set; }
        [Parameter]
        [Required]
        public string ConfirmPassword { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        public void Cancel()
        {
            MudDialog.Cancel();
        }
        private async Task SaveAsync()
        {
            form.Validate();
            if (form.IsValid)
            {
                
                MudDialog.Close();
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
        bool PasswordVisibility;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if(PasswordVisibility)
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
    }
}
