using System.Diagnostics.CodeAnalysis;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Wrapper;
using LazyCache;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.DocumentManagement.DocumentTypes.Commands;

[ExcludeFromCodeCoverage]
public sealed record DeleteDocumentTypeCommand(int Id) : ICommand<Result<int>>;

internal sealed class DeleteDocumentTypeCommandHandler : ICommandHandler<DeleteDocumentTypeCommand, Result<int>>
{
    private readonly IDocumentRepository _documentRepository;
    private readonly IDocumentTypeRepository _documentTypeRepository;
    private readonly IStringLocalizer<DeleteDocumentTypeCommandHandler> _localizer;
    private readonly IAppCache _cache;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDocumentTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IDocumentRepository documentRepository,
        IDocumentTypeRepository documentTypeRepository,
        IStringLocalizer<DeleteDocumentTypeCommandHandler> localizer,
        IAppCache cache)
    {
        _unitOfWork = unitOfWork;
        _documentRepository = documentRepository;
        _documentTypeRepository = documentTypeRepository;
        _localizer = localizer;
        _cache = cache;
    }

    public async Task<Result<int>> Handle(DeleteDocumentTypeCommand command, CancellationToken cancellationToken)
    {
        var isDocumentTypeUsed = await _documentRepository.IsDocumentTypeUsedAsync(command.Id);
        if (isDocumentTypeUsed)
        {
            return Result.Fail<int>(_localizer["Deletion Not Allowed"]);
        }

        var documentType = await _documentTypeRepository.GetByIdAsync(command.Id, cancellationToken);
        if (documentType == null)
        {
            return Result.Fail<int>(_localizer["Document Type Not Found!"]);
        }

        _documentTypeRepository.Remove(documentType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _cache.Remove(ApplicationConstants.Cache.GetAllDocumentTypesCacheKey);
        return Result.Ok(documentType.Id, _localizer["Document Type Deleted"]);
    }
}
