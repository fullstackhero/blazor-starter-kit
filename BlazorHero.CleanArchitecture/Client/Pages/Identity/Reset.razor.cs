using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components;
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
        [Inject] private Microsoft.Extensions.Localization.IStringLocalizer<Reset> localizer { get; set; }

        private FluentValidationValidator _fluentValidationValidator;
        private bool validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

        private readonly ResetPasswordRequest resetPasswordModel = new();

        protected override void OnInitialized()
        {
            var uri = _navigationManager.ToAbsoluteUri(_navigationManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("Token", out var param))
            {
                var queryToken = param.First();
                resetPasswordModel.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(queryToken));
            }
        }

        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(resetPasswordModel.Token))
            {
                var result = await _userManager.ResetPasswordAsync(resetPasswordModel);
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
                _snackBar.Add(localizer["Token Not Found!"], Severity.Error);
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