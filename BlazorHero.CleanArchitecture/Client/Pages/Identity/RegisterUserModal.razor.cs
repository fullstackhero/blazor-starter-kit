using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;
using Blazored.FluentValidation;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class RegisterUserModal
    {
        [Inject] private Microsoft.Extensions.Localization.IStringLocalizer<RegisterUserModal> localizer { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private readonly RegisterRequest registerUserModel = new();

        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

        private void Cancel()
        {
            MudDialog.Cancel();
        }

        private async Task SubmitAsync()
        {
            var response = await _userManager.RegisterUserAsync(registerUserModel);
            if (response.Succeeded)
            {
                _snackBar.Add(response.Messages[0], Severity.Success);
                MudDialog.Close();
            }
            else
            {
                foreach (var message in response.Messages)
                {
                    _snackBar.Add(message, Severity.Error);
                }
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
    }
}