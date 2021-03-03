using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Authentication
{
    public partial class Login
    {
        private TokenRequest model = new TokenRequest();
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            var state = await _authState.GetAuthenticationStateAsync();
            if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                _navigationManager.NavigateTo("/");
            }
        }

        private async Task SubmitAsync()
        {
            var result = await _authService.Login(model);
            if (result.Succeeded)
            {
                _navigationManager.NavigateTo("/", true);
                _snackBar.Add($"Welcome {model.Email}", Severity.Success);
            }
            else
            {
                 _snackBar.Add(result.Message, Severity.Error);
            }
        }
        private void FillAdminstratorCredentials()
        {
            model.Email = "mukesh@blazorhero.com";
            model.Password = "123Pa$$word!";
        }
        private void FillBasicUserCredentials()
        {
            model.Email = "john@blazorhero.com";
            model.Password = "123Pa$$word!";
        }
    }
}