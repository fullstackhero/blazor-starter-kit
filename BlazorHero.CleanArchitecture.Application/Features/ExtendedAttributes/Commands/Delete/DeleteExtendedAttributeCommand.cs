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
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>
            where TId : IEquatable<TId>
    {
        public TId Id { get; set; }
    }

    internal class DeleteExtendedAttributeCommandHandler<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequestHandler<DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>, Result<TId>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>
            where TId : IEquatable<TId>
    {
        private readonly IStringLocalizer<DeleteExtendedAttributeCommandLocalization> _localizer;
        private readonly IUnitOfWork<TEntityId> _entityUnitOfWork;
        private readonly IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> _unitOfWork;

        public DeleteExtendedAttributeCommandHandler(
            IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> unitOfWork,
            IStringLocalizer<DeleteExtendedAttributeCommandLocalization> localizer,
            IUnitOfWork<TEntityId> entityUnitOfWork)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _entityUnitOfWork = entityUnitOfWork;
        }

        public async Task<Result<TId>> Handle(DeleteExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> command, CancellationToken cancellationToken)
        {
            var isExtendedAttributeUsed = await _entityUnitOfWork.Repository<TEntity>().Entities
                .Include(x => x.ExtendedAttributes)
                .AnyAsync(x => x.ExtendedAttributes.Any(e => e.Id.Equals(command.Id)), cancellationToken);
            if (!isExtendedAttributeUsed)
            {
                var extendedAttribute = await _unitOfWork.Repository<TExtendedAttribute>().GetByIdAsync(command.Id);
                if (extendedAttribute != null)
                {
                    await _unitOfWork.Repository<TExtendedAttribute>().DeleteAsync(extendedAttribute);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllEntityExtendedAttributesCacheKey(typeof(TEntity).Name));

                    var cacheKeys = await _unitOfWork.Repository<TExtendedAttribute>().Entities.Select(x =>
                        ApplicationConstants.Cache.GetAllEntityExtendedAttributesByEntityIdCacheKey(
                            typeof(TEntity).Name, x.Entity.Id)).Distinct().ToArrayAsync(cancellationToken);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, cacheKeys);

                    return await Result<TId>.SuccessAsync(extendedAttribute.Id, _localizer["Extended Attribute Deleted"]);
                }
                else
                {
                    return await Result<TId>.FailAsync(_localizer["Extended Attribute Not Found!"]);
                }
            }
            else
            {
                return await Result<TId>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}