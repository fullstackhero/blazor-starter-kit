using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Contracts.Identity;

public class TokenRequestValidator : AbstractValidator<TokenRequest>
{
    public TokenRequestValidator(IStringLocalizer<TokenRequestValidator> localizer)
    {
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage(localizer["Email is required"])
            .EmailAddress().WithMessage(localizer["Email is not correct"]);
        RuleFor(request => request.Password)
            .NotEmpty().WithMessage(localizer["Password is required!"]);
    }
}
