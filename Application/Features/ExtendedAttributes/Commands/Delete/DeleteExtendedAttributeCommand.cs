using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.Delete
{
    internal class DeleteExtendedAttributeCommandLocalization
    {
        // for localization
    }

    public class DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequest<Result<TId>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        public TId Id { get; set; }
    }

    internal class DeleteExtendedAttributeCommandHandler<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequestHandler<DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>, Result<TId>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        private readonly IStringLocalizer<DeleteExtendedAttributeCommandLocalization> _localizer;
        private readonly IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> _unitOfWork;

        public DeleteExtendedAttributeCommandHandler(
            IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> unitOfWork,
            IStringLocalizer<DeleteExtendedAttributeCommandLocalization> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<TId>> Handle(DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> command, CancellationToken cancellationToken)
        {
            var extendedAttribute = await _unitOfWork.Repository<TExtendedAttribute>().GetByIdAsync(command.Id);
            if (extendedAttribute != null)
            {
                await _unitOfWork.Repository<TExtendedAttribute>().DeleteAsync(extendedAttribute);

                // delete all caches related with deleted entity extended attribute
                var cacheKeys = await _unitOfWork.Repository<TExtendedAttribute>().Entities.Select(x =>
                    ApplicationConstants.Cache.GetAllEntityExtendedAttributesByEntityIdCacheKey(
                        typeof(TEntity).Name, x.Entity.Id)).Distinct().ToListAsync(cancellationToken);
                cacheKeys.Add(ApplicationConstants.Cache.GetAllEntityExtendedAttributesCacheKey(typeof(TEntity).Name));
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, cacheKeys.ToArray());

                return await Result<TId>.SuccessAsync(extendedAttribute.Id, _localizer["Extended Attribute Deleted"]);
            }
            else
            {
                return await Result<TId>.FailAsync(_localizer["Extended Attribute Not Found!"]);
            }
        }
    }
}