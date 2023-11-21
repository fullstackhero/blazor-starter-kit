using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Contracts.Identity;

public class ForgotPasswordRequestValidator : AbstractValidator<ForgotPasswordRequest>
{
    public ForgotPasswordRequestValidator(IStringLocalizer<ForgotPasswordRequestValidator> localizer) =>
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage(localizer["Email is required"])
            .EmailAddress().WithMessage(localizer["Email is not correct"]);
}
