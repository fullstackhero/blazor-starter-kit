using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blazored.FluentValidation;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Reset
    {
        private FluentValidationValidator _fluentValidationValidator;
        private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
        private readonly ResetPasswordRequest _resetPasswordModel = new();

        protected override void OnInitialized()
        {
            var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out var param))
            {
                var queryToken = param.First();
                _resetPasswordModel.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(queryToken));
            }
        }

        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(_resetPasswordModel.Token))
            {
                var result = await _userManager.ResetPasswordAsync(_resetPasswordModel);
                if (result.Succeeded)
                {
                    _snackBar.Add(result.Messages[0], Severity.Success);
                    _navigationManager.NavigateTo("/");
                }
                else
                {
                    foreach (var message in result.Messages)
                    {
                        _snackBar.Add(message, Severity.Error);
                    }
                }
            }
            else
            {
                _snackBar.Add(_localizer["Token Not Found!"], Severity.Error);
            }
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

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
}