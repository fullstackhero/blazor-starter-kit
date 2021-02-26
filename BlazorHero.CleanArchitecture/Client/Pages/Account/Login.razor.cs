using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Account
{
    public partial class Login
    {
        LoginRequest model = new LoginRequest();
        public bool ShowErrors { get; set; }

        public IEnumerable<string> Errors { get; set; }
        private async Task SubmitAsync()
        {
            var result = await this.AuthService.Login(this.model);

            if (result.Succeeded)
            {
                this.ShowErrors = false;
                this.NavigationManager.NavigateTo("/");
            }
            else
            {
                this.Errors = result.Errors;
                this.ShowErrors = true;
            }
        }
    }
}
