using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Contracts;
using CleanBlazor.Domain.Entities.Misc;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Wrapper;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.DocumentManagement.Documents.Commands;

[ExcludeFromCodeCoverage]
public sealed record AddEditDocumentCommand : ICommand<Result<int>>
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public bool IsPublic { get; init; }
    public string Url { get; set; }
    public int DocumentTypeId { get; init; }
    public UploadRequest UploadRequest { get; init; }
}

internal sealed class AddEditDocumentCommandHandler : ICommandHandler<AddEditDocumentCommand, Result<int>>
{
    private readonly IStringLocalizer<AddEditDocumentCommandHandler> _localizer;
    private readonly IDocumentRepository _documentRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUploadService _uploadService;

    public AddEditDocumentCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IUploadService uploadService,
        IStringLocalizer<AddEditDocumentCommandHandler> localizer,
        IDocumentRepository documentRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _uploadService = uploadService;
        _localizer = localizer;
        _documentRepository = documentRepository;
    }

    public async Task<Result<int>> Handle(AddEditDocumentCommand command, CancellationToken cancellationToken)
    {
        UploadRequest uploadRequest = command.UploadRequest;
        if (uploadRequest != null)
        {
            uploadRequest.FileName = $"D-{Guid.NewGuid()}{uploadRequest.Extension}";
        }

        if (command.Id == 0)
        {
            var doc = _mapper.Map<Document>(command);
            if (uploadRequest != null)
            {
                doc.Url = _uploadService.UploadAsync(uploadRequest);
            }

            _documentRepository.Add(doc);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(doc.Id, _localizer["Document Saved"]);
        }
        else
        {
            var doc = await _documentRepository.GetByIdAsync(command.Id, cancellationToken);
            if (doc == null)
            {
                return Result.Fail<int>(_localizer["Document Not Found!"]);
            }

            doc.Title = command.Title ?? doc.Title;
            doc.Description = command.Description ?? doc.Description;
            doc.IsPublic = command.IsPublic;
            if (uploadRequest != null)
            {
                doc.Url = _uploadService.UploadAsync(uploadRequest);
            }

            doc.DocumentTypeId = command.DocumentTypeId == 0 ? doc.DocumentTypeId : command.DocumentTypeId;
            _documentRepository.Update(doc);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(doc.Id, _localizer["Document Updated"]);
        }
    }
}
