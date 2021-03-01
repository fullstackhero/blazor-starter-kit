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
        private LoginRequest model = new LoginRequest();
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        protected override async Task OnInitializedAsync()
        {
            // redirect to home if already logged in
            var state = await _authState.GetAuthenticationStateAsync();
            if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            {
                _navigationManager.NavigateTo("/");
            }
        }

        private async Task SubmitAsync()
        {
            var result = await _authService.Login(model).ConfigureAwait(false);
            if (result.Succeeded)
            {
                _snackBar.Add($"Welcome {model.Email}", Severity.Success);
                _navigationManager.NavigateTo("/", true);
            }
            else
            {
                Errors = result.Errors;
                foreach (string error in Errors)
                {
                    _snackBar.Add(error, Severity.Error);
                }
            }
        }
    }
}