using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.Documents.Commands.Delete
{
    public class DeleteDocumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Result<int>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IStringLocalizer<DeleteDocumentCommandHandler> _localizer;

            public DeleteDocumentCommandHandler(IUnitOfWork unitOfWork, IStringLocalizer<DeleteDocumentCommandHandler> localizer)
            {
                _unitOfWork = unitOfWork;
                _localizer = localizer;
            }

            public async Task<Result<int>> Handle(DeleteDocumentCommand command, CancellationToken cancellationToken)
            {
                var document = await _unitOfWork.Repository<Document>().GetByIdAsync(command.Id);
                await _unitOfWork.Repository<Document>().DeleteAsync(document);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(document.Id, _localizer["Document Deleted"]);
            }
        }
    }
}