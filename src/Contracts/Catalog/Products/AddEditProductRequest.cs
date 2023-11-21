using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Contracts.Catalog.Products;

[ExcludeFromCodeCoverage]
public sealed class AddEditProductRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
    public string Description { get; set; }
    public string ImageDataUrl { get; set; }
    public decimal Rate { get; set; }
    public int BrandId { get; set; }
    public UploadRequest UploadRequest { get; set; }
}

public sealed class AddEditProductCommandValidator : AbstractValidator<AddEditProductRequest>
{
    public AddEditProductCommandValidator(IStringLocalizer<AddEditProductCommandValidator> localizer)
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage(localizer["Name is required!"]);
        RuleFor(request => request.Barcode)
            .NotEmpty().WithMessage(localizer["Barcode is required!"]);
        RuleFor(request => request.Description)
            .NotEmpty().WithMessage(localizer["Description is required!"]);
        RuleFor(request => request.BrandId)
            .GreaterThan(0).WithMessage(localizer["Brand is required!"]);
        RuleFor(request => request.Rate)
            .GreaterThan(0).WithMessage(localizer["Rate must be greater than 0"]);
    }
}
