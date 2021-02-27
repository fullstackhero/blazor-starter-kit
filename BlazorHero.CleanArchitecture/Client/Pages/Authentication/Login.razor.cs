using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Authentication
{
    public partial class Login
    {
        private LoginRequest model = new LoginRequest();
        public bool ShowErrors { get; set; }

        public IEnumerable<string> Errors { get; set; } = new List<string>();

        private async Task SubmitAsync()
        {
            var result = await _authService.Login(this.model);

            if (result.Succeeded)
            {
                this.ShowErrors = false;
                _navigationManager.NavigateTo("/");
                _snackBar.Add("Logged In", Severity.Success);
            }
            else
            {
                this.Errors = result.Errors;
                foreach (string error in Errors)
                {
                    _snackBar.Add(error, Severity.Error);
                }
                this.ShowErrors = true;
            }
        }
    }
}