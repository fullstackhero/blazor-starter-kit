using System.Net.Http.Json;
using System.Text;
using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Identity;

public partial class Reset
{
    private readonly ResetPasswordRequest _resetPasswordModel = new();
    private FluentValidationValidator _fluentValidationValidator;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _passwordVisibility;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    protected override void OnInitialized()
    {
        Uri uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
        if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out StringValues param))
        {
            var queryToken = param.First();
            _resetPasswordModel.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(queryToken));
        }
    }

    private async Task SubmitAsync()
    {
        if (!string.IsNullOrEmpty(_resetPasswordModel.Token))
        {
            await HttpClient
                .PostAsJsonAsync<ResetPasswordRequest, Result>(UsersEndpoints.ResetPassword, _resetPasswordModel)
                .Match(message =>
                    {
                        SnackBar.Success(message);
                        NavigationManager.NavigateTo("/");
                    },
                    errors => SnackBar.Error(errors));
        }
        else
        {
            SnackBar.Error(Localizer["Token Not Found!"]);
        }
    }

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
