using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using CleanBlazor.Application.Abstractions.Messaging;
using CleanBlazor.Application.Abstractions.Persistence;
using CleanBlazor.Domain.Entities.Misc;
using CleanBlazor.Domain.Repositories;
using CleanBlazor.Shared.Constants.Application;
using CleanBlazor.Shared.Wrapper;
using LazyCache;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Application.Features.DocumentManagement.DocumentTypes.Commands;

[ExcludeFromCodeCoverage]
public sealed record AddEditDocumentTypeCommand(int Id, string Name, string Description) : ICommand<Result<int>>;

internal sealed class AddEditDocumentTypeCommandHandler : ICommandHandler<AddEditDocumentTypeCommand, Result<int>>
{
    private readonly IStringLocalizer<AddEditDocumentTypeCommandHandler> _localizer;
    private readonly IDocumentTypeRepository _documentTypeRepository;
    private readonly IAppCache _cache;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AddEditDocumentTypeCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IStringLocalizer<AddEditDocumentTypeCommandHandler> localizer,
        IDocumentTypeRepository documentTypeRepository,
        IAppCache cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _localizer = localizer;
        _documentTypeRepository = documentTypeRepository;
        _cache = cache;
    }

    public async Task<Result<int>> Handle(AddEditDocumentTypeCommand command, CancellationToken cancellationToken)
    {
        if (await _documentTypeRepository.Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Name == command.Name, cancellationToken))
        {
            return Result.Fail<int>(_localizer["Document type with this name already exists."]);
        }

        if (command.Id == 0)
        {
            var documentType = _mapper.Map<DocumentType>(command);
            _documentTypeRepository.Add(documentType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove(ApplicationConstants.Cache.GetAllDocumentTypesCacheKey);
            return Result.Ok(documentType.Id, _localizer["Document Type Saved"]);
        }
        else
        {
            var documentType = await _documentTypeRepository.GetByIdAsync(command.Id, cancellationToken);
            if (documentType == null)
            {
                return Result.Fail<int>(_localizer["Document Type Not Found!"]);
            }

            documentType.Name = command.Name ?? documentType.Name;
            documentType.Description = command.Description ?? documentType.Description;
            _documentTypeRepository.Update(documentType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            _cache.Remove(ApplicationConstants.Cache.GetAllDocumentTypesCacheKey);
            return Result.Ok(documentType.Id, _localizer["Document Type Updated"]);
        }
    }
}
