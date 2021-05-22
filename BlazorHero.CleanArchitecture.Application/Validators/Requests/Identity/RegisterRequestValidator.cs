using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Validators.Requests.Identity
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IStringLocalizer<RegisterRequestValidator> localizer)
        {
            RuleFor(request => request.FirstName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["First Name is required"]);
            RuleFor(request => request.LastName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Last Name is required"]);
            RuleFor(request => request.Email)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Email is required"])
                .EmailAddress().WithMessage(x => localizer["Email is not correct"]);
            RuleFor(request => request.UserName)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["UserName is required"])
                .MinimumLength(6).WithMessage(localizer["UserName must be at least of length 6"]);
            RuleFor(request => request.Password)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Password is required!"])
                .MinimumLength(8).WithMessage(localizer["Password must be at least of length 8"])
                .Matches(@"[A-Z]").WithMessage(localizer["Password must contain at least one capital letter"])
                .Matches(@"[a-z]").WithMessage(localizer["Password must contain at least one lowercase letter"])
                .Matches(@"[0-9]").WithMessage(localizer["Password must contain at least one digit"]);
            RuleFor(request => request.ConfirmPassword)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage(x => localizer["Password Confirmation is required!"])
                .Equal(request => request.Password).WithMessage(x => localizer["Passwords don't match"]);
        }
    }
}