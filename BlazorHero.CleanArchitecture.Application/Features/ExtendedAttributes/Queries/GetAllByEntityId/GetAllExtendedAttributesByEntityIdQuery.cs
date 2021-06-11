using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetAllByEntityId
{
    public class GetAllExtendedAttributesByEntityIdQuery<TId, TEntityId>
        : IRequest<Result<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>>
    {
        public TEntityId EntityId { get; set; }

        public GetAllExtendedAttributesByEntityIdQuery(TEntityId entityId)
        {
            EntityId = entityId;
        }
    }

    internal class GetAllExtendedAttributesByEntityIdQueryHandler<TId, TEntityId>
        : IRequestHandler<GetAllExtendedAttributesByEntityIdQuery<TId, TEntityId>, Result<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>>
    {
        private readonly IExtendedAttributeRepository<TId, TEntityId> _repository;

        public GetAllExtendedAttributesByEntityIdQueryHandler(IExtendedAttributeRepository<TId, TEntityId> repository)
        {
            _repository = repository;
        }

        public async Task<Result<List<GetAllExtendedAttributesByEntityIdResponse<TId, TEntityId>>>> Handle(GetAllExtendedAttributesByEntityIdQuery<TId, TEntityId> request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllByEntityIdAsync(request.EntityId);
        }
    }
}