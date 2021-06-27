using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.GetById
{
    public class GetExtendedAttributeByIdQuery<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequest<Result<GetExtendedAttributeByIdResponse<TId, TEntityId>>>
        where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
        where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
        where TId : IEquatable<TId>
    {
        public TId Id { get; set; }
    }

    internal class GetExtendedAttributeByIdQueryHandler<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequestHandler<GetExtendedAttributeByIdQuery<TId, TEntityId, TEntity, TExtendedAttribute>, Result<GetExtendedAttributeByIdResponse<TId, TEntityId>>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        private readonly IUnitOfWork<TId> _unitOfWork;
        private readonly IMapper _mapper;

        public GetExtendedAttributeByIdQueryHandler(IUnitOfWork<TId> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetExtendedAttributeByIdResponse<TId, TEntityId>>> Handle(GetExtendedAttributeByIdQuery<TId, TEntityId, TEntity, TExtendedAttribute> query, CancellationToken cancellationToken)
        {
            var extendedAttribute = await _unitOfWork.Repository<TExtendedAttribute>().GetByIdAsync(query.Id);
            var mappedExtendedAttribute = _mapper.Map<GetExtendedAttributeByIdResponse<TId, TEntityId>>(extendedAttribute);
            return await Result<GetExtendedAttributeByIdResponse<TId, TEntityId>>.SuccessAsync(mappedExtendedAttribute);
        }
    }
}