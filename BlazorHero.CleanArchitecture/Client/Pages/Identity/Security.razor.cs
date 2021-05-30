using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using Blazored.FluentValidation;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Security
    {
        [Inject] private Microsoft.Extensions.Localization.IStringLocalizer<Security> localizer { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private readonly ChangePasswordRequest passwordModel = new();

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
                    _snackBar.Add(error, Severity.Error);
                }
            }
        }

        private bool CurrentPasswordVisibility;
        private InputType CurrentPasswordInput = InputType.Password;
        private string CurrentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private bool NewPasswordVisibility;
        private InputType NewPasswordInput = InputType.Password;
        private string NewPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private void TogglePasswordVisibility(bool newPassword)
        {
            if (newPassword)
            {
                if (NewPasswordVisibility)
                {
                    NewPasswordVisibility = false;
                    NewPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                    NewPasswordInput = InputType.Password;
                }
                else
                {
                    NewPasswordVisibility = true;
                    NewPasswordInputIcon = Icons.Material.Filled.Visibility;
                    NewPasswordInput = InputType.Text;
                }
            }
            else
            {
                if (CurrentPasswordVisibility)
                {
                    CurrentPasswordVisibility = false;
                    CurrentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                    CurrentPasswordInput = InputType.Password;
                }
                else
                {
                    CurrentPasswordVisibility = true;
                    CurrentPasswordInputIcon = Icons.Material.Filled.Visibility;
                    CurrentPasswordInput = InputType.Text;
                }
            }
        }
    }
}