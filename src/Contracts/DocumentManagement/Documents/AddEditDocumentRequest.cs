using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Contracts.Documents;

[ExcludeFromCodeCoverage]
public sealed class AddEditDocumentRequest
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public string Url { get; set; }
    public int DocumentTypeId { get; set; }
    public UploadRequest UploadRequest { get; set; }
}

public sealed class AddEditDocumentCommandValidator : AbstractValidator<AddEditDocumentRequest>
{
    public AddEditDocumentCommandValidator(IStringLocalizer<AddEditDocumentCommandValidator> localizer)
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage(localizer["Title is required!"]);
        RuleFor(request => request.Description)
            .NotEmpty().WithMessage(localizer["Description is required!"]);
        RuleFor(request => request.DocumentTypeId)
            .GreaterThan(0).WithMessage(localizer["Document Type is required!"]);
        RuleFor(request => request.Url)
            .NotEmpty().WithMessage(localizer["File is required!"]);
    }
}
