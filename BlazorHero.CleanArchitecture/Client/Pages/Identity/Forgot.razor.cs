using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using MudBlazor;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Forgot
    {
        [Inject] private Microsoft.Extensions.Localization.IStringLocalizer<Forgot> localizer { get; set; }

        private readonly ForgotPasswordRequest emailModel = new ForgotPasswordRequest();

        private async Task SubmitAsync()
        {
            var result = await _userManager.ForgotPasswordAsync(emailModel);
            if (result.Succeeded)
            {
                _snackBar.Add(localizer["Done!"], Severity.Success);
                _navigationManager.NavigateTo("/");
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    _snackBar.Add(localizer[message], Severity.Error);
                }
            }
        }
    }
}