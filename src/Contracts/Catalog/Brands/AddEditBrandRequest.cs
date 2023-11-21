using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Contracts.Catalog.Brands;

[ExcludeFromCodeCoverage]
public sealed class AddEditBrandRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }
}

public sealed class AddEditBrandRequestValidator : AbstractValidator<AddEditBrandRequest>
{
    public AddEditBrandRequestValidator(IStringLocalizer<AddEditBrandRequestValidator> localizer)
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage(localizer["Name is required!"]);
        RuleFor(request => request.Description)
            .NotEmpty().WithMessage(localizer["Description is required!"]);
        RuleFor(request => request.Tax)
            .GreaterThan(0).WithMessage(localizer["Tax must be greater than 0"]);
    }
}
