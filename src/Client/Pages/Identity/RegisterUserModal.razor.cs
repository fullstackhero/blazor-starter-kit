using System.Net.Http.Json;
using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Identity;

public partial class RegisterUserModal
{
    private readonly RegisterRequest _registerUserModel = new();
    private FluentValidationValidator _fluentValidationValidator;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _passwordVisibility;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
    [CascadingParameter] private MudDialogInstance MudDialog { get; set; }

    private void Cancel() => MudDialog.Cancel();

    private async Task SubmitAsync() =>
        await HttpClient.PostAsJsonAsync<RegisterRequest, Result>(UsersEndpoints.Register, _registerUserModel)
            .Match(message =>
                {
                    SnackBar.Success(message);
                    MudDialog.Close();
                },
                errors => SnackBar.Error(errors));

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }
}
