using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Commands.Delete
{
    public class DeleteDocumentExtendedAttributeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDocumentExtendedAttributeCommandHandler : IRequestHandler<DeleteDocumentExtendedAttributeCommand, Result<int>>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IStringLocalizer<DeleteDocumentExtendedAttributeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteDocumentExtendedAttributeCommandHandler(IUnitOfWork<int> unitOfWork, IDocumentRepository documentRepository, IStringLocalizer<DeleteDocumentExtendedAttributeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _documentRepository = documentRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDocumentExtendedAttributeCommand command, CancellationToken cancellationToken)
        {
            var isDocumentExtendedAttributeUsed = await _documentRepository.IsDocumentExtendedAttributeUsed(command.Id);
            if (!isDocumentExtendedAttributeUsed)
            {
                var documentExtendedAttribute = await _unitOfWork.Repository<DocumentExtendedAttribute>().GetByIdAsync(command.Id);
                if (documentExtendedAttribute != null)
                {
                    await _unitOfWork.Repository<DocumentExtendedAttribute>().DeleteAsync(documentExtendedAttribute);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllDocumentExtendedAttributesCacheKey);
                    return await Result<int>.SuccessAsync(documentExtendedAttribute.Id, _localizer["Document Extended Attribute Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Document Extended Attribute Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}