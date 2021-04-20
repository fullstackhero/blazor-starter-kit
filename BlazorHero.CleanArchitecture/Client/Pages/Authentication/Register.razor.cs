using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Authentication
{
    public partial class Register
    {
        private bool success;
        private string[] errors = { };
        private MudForm form;

        private async Task SubmitAsync()
        {
            form.Validate();
            if (form.IsValid)
            {
                var request = new RegisterRequest()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    ActivateUser = true,
                    AutoConfirmEmail = false
                };
                var response = await _userManager.RegisterUserAsync(request);
                if (response.Succeeded)
                {
                    _snackBar.Add(localizer[response.Messages[0]], Severity.Success);
                    _navigationManager.NavigateTo("/login");
                    model = new RegisterUsermodel();
                }
                else
                {
                    foreach (var message in response.Messages)
                    {
                        _snackBar.Add(localizer[message], Severity.Error);
                    }
                }
            }
            else
            {
                foreach (var message in form.Errors)
                {
                    _snackBar.Add(localizer[message], Severity.Error);
                }
            }
        }

        private RegisterUsermodel model { get; set; } = new RegisterUsermodel();

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

        private class RegisterUsermodel
        {
            [Parameter] [Required] [MinLength(6)] public string UserName { get; set; }

            [Parameter] [Required] public string FirstName { get; set; }

            [Parameter] [Required] public string LastName { get; set; }

            [Parameter] [Required] [EmailAddress] public string Email { get; set; }

            [Parameter] [Required] public string Password { get; set; }

            [Parameter] [Required] public string ConfirmPassword { get; set; }
        }
    }
}