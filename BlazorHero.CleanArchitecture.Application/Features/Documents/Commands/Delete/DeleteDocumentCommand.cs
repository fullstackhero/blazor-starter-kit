using System.Linq;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.Misc;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Documents.Commands.Delete
{
    public class DeleteDocumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteDocumentCommandHandler> _localizer;

        public DeleteDocumentCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDocumentCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDocumentCommand command, CancellationToken cancellationToken)
        {
            var documentsWithExtendedAttributes = _unitOfWork.Repository<Document>().Entities.Include(x => x.ExtendedAttributes);

            var document = await _unitOfWork.Repository<Document>().GetByIdAsync(command.Id);
            if (document != null)
            {
                await _unitOfWork.Repository<Document>().DeleteAsync(document);

                // delete all caches related with deleted entity
                var cacheKeys = await documentsWithExtendedAttributes.SelectMany(x => x.ExtendedAttributes).Where(x => x.EntityId == command.Id).Distinct().Select(x => ApplicationConstants.Cache.GetAllEntityExtendedAttributesByEntityIdCacheKey(nameof(Document), x.EntityId))
                    .ToListAsync(cancellationToken);
                cacheKeys.Add(ApplicationConstants.Cache.GetAllEntityExtendedAttributesCacheKey(nameof(Document)));
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, cacheKeys.ToArray());

                return await Result<int>.SuccessAsync(document.Id, _localizer["Document Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Document Not Found!"]);
            }
        }
    }
}