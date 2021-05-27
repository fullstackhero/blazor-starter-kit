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
    }
}