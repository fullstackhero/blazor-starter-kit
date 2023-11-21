using System.Net.Http.Json;
using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Identity;

public partial class Security
{
    private readonly ChangePasswordRequest _passwordModel = new();
    private InputType _currentPasswordInput = InputType.Password;
    private string _currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _currentPasswordVisibility;
    private FluentValidationValidator _fluentValidationValidator;
    private InputType _newPasswordInput = InputType.Password;
    private string _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _newPasswordVisibility;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    private async Task ChangePasswordAsync()
    {
        await HttpClient
            .PutAsJsonAsync<ChangePasswordRequest, Result>(AccountsEndpoints.ChangePassword, _passwordModel)
            .Match(_ =>
                {
                    SnackBar.Success(Localizer["Password Changed!"]);
                    _passwordModel.Password = string.Empty;
                    _passwordModel.NewPassword = string.Empty;
                    _passwordModel.ConfirmNewPassword = string.Empty;
                },
                errors => SnackBar.Error(errors));
    }

    private void TogglePasswordVisibility(bool newPassword)
    {
        if (newPassword)
        {
            if (_newPasswordVisibility)
            {
                _newPasswordVisibility = false;
                _newPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                _newPasswordInput = InputType.Password;
            }
            else
            {
                _newPasswordVisibility = true;
                _newPasswordInputIcon = Icons.Material.Filled.Visibility;
                _newPasswordInput = InputType.Text;
            }
        }
        else
        {
            if (_currentPasswordVisibility)
            {
                _currentPasswordVisibility = false;
                _currentPasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                _currentPasswordInput = InputType.Password;
            }
            else
            {
                _currentPasswordVisibility = true;
                _currentPasswordInputIcon = Icons.Material.Filled.Visibility;
                _currentPasswordInput = InputType.Text;
            }
        }
    }
}
