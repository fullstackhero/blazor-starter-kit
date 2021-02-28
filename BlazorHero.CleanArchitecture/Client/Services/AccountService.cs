using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using BlazorHero.CleanArchitecture.Client.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services
{
    public class AccountService :  IAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;
        private readonly ISnackbar _snackBar;
        private readonly NavigationManager _navigationManager;
        public IEnumerable<string> Errors { get; set; }

        public AccountService(HttpClient httpClient, IAuthService authService, ISnackbar snackBar, NavigationManager navigationManager)
        {
            _httpClient = httpClient;
            _authService = authService;
            _snackBar = snackBar;
            _navigationManager = navigationManager;
        }

        public async Task ChangePasswordAsync(ChangePasswordRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync(Constants.APIRoutes.ChangePassword, model);

            if (response.IsSuccessStatusCode)
            {
                model.Password = null;
                model.NewPassword = null;
                model.ConfirmNewPassword = null;
                await _authService.Logout();
                _snackBar.Add("Your password has been changed successfully.Please login.", Severity.Success);
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                Errors = await response.Content.ReadFromJsonAsync<string[]>();
                foreach(string error in Errors)
                {
                    _snackBar.Add(error, Severity.Error);
                }
            }
        }

        public async Task UpdateProfiledAsync(UpdateProfileRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync(Constants.APIRoutes.UpdateProfile, model);

            if (response.IsSuccessStatusCode)
            {
                _snackBar.Add("Profile Updated.", Severity.Success);
                await _authService.Logout();
                _navigationManager.NavigateTo("/login");
            }
            else
            {
                Errors = await response.Content.ReadFromJsonAsync<string[]>();
                foreach (string error in Errors)
                {
                    _snackBar.Add(error, Severity.Error);
                }
            }
        }
    }
}
