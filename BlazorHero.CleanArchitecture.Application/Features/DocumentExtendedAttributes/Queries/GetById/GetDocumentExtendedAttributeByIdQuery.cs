using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;

namespace BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.GetById
{
    public class GetDocumentExtendedAttributeByIdQuery : IRequest<Result<GetDocumentExtendedAttributeByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetDocumentExtendedAttributeByIdQueryHandler : IRequestHandler<GetDocumentExtendedAttributeByIdQuery, Result<GetDocumentExtendedAttributeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDocumentExtendedAttributeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetDocumentExtendedAttributeByIdResponse>> Handle(GetDocumentExtendedAttributeByIdQuery query, CancellationToken cancellationToken)
        {
            var documentExtendedAttribute = await _unitOfWork.Repository<DocumentExtendedAttribute>().GetByIdAsync(query.Id);
            var mappedDocumentType = _mapper.Map<GetDocumentExtendedAttributeByIdResponse>(documentExtendedAttribute);
            return await Result<GetDocumentExtendedAttributeByIdResponse>.SuccessAsync(mappedDocumentType);
        }
    }
}