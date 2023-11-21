using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Contracts.Identity;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator(IStringLocalizer<ChangePasswordRequestValidator> localizer)
    {
        RuleFor(request => request.Password)
            .NotEmpty().WithMessage(localizer["Current Password is required!"]);
        RuleFor(request => request.NewPassword)
            .NotEmpty().WithMessage(localizer["Password is required!"])
            .MinimumLength(8).WithMessage(localizer["Password must be at least of length 8"])
            .Matches("[A-Z]").WithMessage(localizer["Password must contain at least one capital letter"])
            .Matches("[a-z]").WithMessage(localizer["Password must contain at least one lowercase letter"])
            .Matches("[0-9]").WithMessage(localizer["Password must contain at least one digit"]);
        RuleFor(request => request.ConfirmNewPassword)
            .NotEmpty().WithMessage(localizer["Password Confirmation is required!"])
            .Equal(request => request.NewPassword).WithMessage(localizer["Passwords don't match"]);
    }
}
