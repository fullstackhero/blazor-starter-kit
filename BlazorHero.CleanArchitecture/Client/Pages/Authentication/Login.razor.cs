using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Pages.Authentication
{
    public partial class Login
    {
        private LoginRequest model = new LoginRequest();
        public IEnumerable<string> Errors { get; set; } = new List<string>();

        private async Task SubmitAsync()
        {
            var result = await _authService.Login(model);
            if (result.Succeeded)
            {
               
                _snackBar.Add($"Welcome {model.Email}", Severity.Success);
                _navigationManager.NavigateTo("/",true);
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