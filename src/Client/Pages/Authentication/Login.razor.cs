using System.Security.Claims;
using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;

namespace CleanBlazor.Client.Pages.Authentication;

public partial class Login
{
    private FluentValidationValidator _fluentValidationValidator;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _passwordVisibility;
    private TokenRequest _tokenModel = new();
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    protected override async Task OnInitializedAsync()
    {
        ClaimsPrincipal user = await StateProvider.GetCurrentUserAsync();
        if (!user.Identity?.IsAuthenticated == true)
        {
            NavigationManager.NavigateTo("/");
        }
    }

    private async Task SubmitAsync() => await AuthenticationManager.LoginAsync(_tokenModel)
        .Match(_ => { }, errors => SnackBar.Error(errors));

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }

    private void FillAdministratorCredentials()
    {
        _tokenModel.Email = "mukesh@blazorhero.com";
        _tokenModel.Password = "123Pa$$word!";
    }

    private void FillBasicUserCredentials()
    {
        _tokenModel.Email = "john@blazorhero.com";
        _tokenModel.Password = "123Pa$$word!";
    }
}
