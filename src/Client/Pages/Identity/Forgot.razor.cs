using Blazored.FluentValidation;
using CleanBlazor.Client.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Routes;
using CleanBlazor.Shared.Wrapper;

namespace CleanBlazor.Client.Pages.Identity;

public partial class Forgot
{
    private readonly ForgotPasswordRequest _emailModel = new();
    private FluentValidationValidator _fluentValidationValidator;
    private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

    private async Task SubmitAsync() =>
        await HttpClient.PostAsJsonAsync<ForgotPasswordRequest, Result>(UsersEndpoints.ForgotPassword, _emailModel)
            .Match(_ =>
                {
                    SnackBar.Success(Localizer["Done!"]);
                    NavigationManager.NavigateTo("/");
                },
                errors => SnackBar.Error(errors));
}
