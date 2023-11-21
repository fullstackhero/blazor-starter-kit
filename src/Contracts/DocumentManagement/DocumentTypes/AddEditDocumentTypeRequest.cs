using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Contracts.Documents;

[ExcludeFromCodeCoverage]
public sealed class AddEditDocumentTypeRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}

public class AddEditDocumentTypeCommandValidator : AbstractValidator<AddEditDocumentTypeRequest>
{
    public AddEditDocumentTypeCommandValidator(IStringLocalizer<AddEditDocumentTypeCommandValidator> localizer)
    {
        RuleFor(request => request.Name)
            .NotEmpty().WithMessage(localizer["Name is required!"]);
        RuleFor(request => request.Description)
            .NotEmpty().WithMessage(localizer["Description is required!"]);
    }
}
