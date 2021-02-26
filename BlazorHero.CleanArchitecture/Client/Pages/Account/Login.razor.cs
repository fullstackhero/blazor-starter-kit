using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Account
{
    public partial class Login
    {
        LoginRequest model = new LoginRequest();
        public bool ShowErrors { get; set; }

        public IEnumerable<string> Errors { get; set; } = new List<string>();
        private async Task SubmitAsync()
        {
            var result = await this.AuthService.Login(this.model);

            if (result.Succeeded)
            {
                this.ShowErrors = false;
                this.NavigationManager.NavigateTo("/");
                SnackBar.Add("Logged In", Severity.Success);
            }
            else
            {
                this.Errors = result.Errors;
                foreach(string error in Errors)
                {
                    SnackBar.Add(error, Severity.Error);
                }
                this.ShowErrors = true;
            }
        }
    }
}
