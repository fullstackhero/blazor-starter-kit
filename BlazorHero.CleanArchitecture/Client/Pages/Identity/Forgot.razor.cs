using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Identity
{
    public partial class Forgot
    {
        private bool success;
        private string[] errors = { };
        private MudForm form;

        [Parameter]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        private async Task SubmitAsync()
        {
            form.Validate();
            if (form.IsValid)
            {
                var request = new ForgotPasswordRequest() { Email = Email };
                var result = await _userManager.ForgotPasswordAsync(request);
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
}