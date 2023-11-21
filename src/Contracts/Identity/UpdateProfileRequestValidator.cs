using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Contracts.Identity;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator(IStringLocalizer<UpdateProfileRequestValidator> localizer)
    {
        RuleFor(request => request.FirstName)
            .NotEmpty().WithMessage(localizer["First Name is required"]);
        RuleFor(request => request.LastName)
            .NotEmpty().WithMessage(localizer["Last Name is required"]);
        RuleFor(request => request.PhoneNumber)
            .NotEmpty().MinimumLength(6).WithMessage("Phone Number is not correct")
            .When(x => x.PhoneNumber is not null);
        RuleFor(request => request.Email)
            .EmailAddress().WithMessage(localizer["Email is not correct"])
            .When(x => !string.IsNullOrWhiteSpace(x.Email));
    }
}
