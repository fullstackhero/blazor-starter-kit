#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;
using BlazorHero.CleanArchitecture.Domain.Enums;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Commands.AddEdit
{
    public class AddEditDocumentExtendedAttributeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public EntityExtendedAttributeType Type { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Key { get; set; }
        public string? Text { get; set; }
        public decimal? Decimal { get; set; }
        public DateTime? DateTime { get; set; }
        public string? Json { get; set; }
        public string? ExternalId { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    internal class AddEditDocumentExtendedAttributeCommandHandler : IRequestHandler<AddEditDocumentExtendedAttributeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditDocumentExtendedAttributeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditDocumentExtendedAttributeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditDocumentExtendedAttributeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDocumentExtendedAttributeCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Key == command.Key, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Document Extended Attribute with this Key already exists."]);
            }

            if (command.Id == 0)
            {
                var documentExtendedAttribute = _mapper.Map<DocumentExtendedAttribute>(command);
                await _unitOfWork.Repository<DocumentExtendedAttribute>().AddAsync(documentExtendedAttribute);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDocumentExtendedAttributesCacheKey);
                return await Result<int>.SuccessAsync(documentExtendedAttribute.Id, _localizer["Document Extended Attribute Saved"]);
            }
            else
            {
                var documentExtendedAttribute = await _unitOfWork.Repository<DocumentExtendedAttribute>().GetByIdAsync(command.Id);
                if (documentExtendedAttribute != null)
                {
                    documentExtendedAttribute.Key = command.Key;
                    documentExtendedAttribute.EntityId = command.EntityId;
                    documentExtendedAttribute.Type = command.Type;
                    documentExtendedAttribute.Text = command.Text ?? documentExtendedAttribute.Text;
                    documentExtendedAttribute.Decimal = command.Decimal ?? documentExtendedAttribute.Decimal;
                    documentExtendedAttribute.DateTime = command.DateTime ?? documentExtendedAttribute.DateTime;
                    documentExtendedAttribute.Json = command.Json ?? documentExtendedAttribute.Json;
                    documentExtendedAttribute.ExternalId = command.ExternalId ?? documentExtendedAttribute.ExternalId;
                    documentExtendedAttribute.Description = command.Description ?? documentExtendedAttribute.Description;
                    documentExtendedAttribute.IsActive = command.IsActive;
                    await _unitOfWork.Repository<DocumentExtendedAttribute>().UpdateAsync(documentExtendedAttribute);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDocumentExtendedAttributesCacheKey);
                    return await Result<int>.SuccessAsync(documentExtendedAttribute.Id, _localizer["Document Extended Attribute Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Document Extended Attribute Not Found!"]);
                }
            }
        }
    }
}