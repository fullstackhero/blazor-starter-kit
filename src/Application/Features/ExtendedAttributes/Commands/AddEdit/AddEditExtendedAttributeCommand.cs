#nullable enable
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Domain.Enums;
using BlazorHero.CleanArchitecture.Shared.Constants.Application;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Commands.AddEdit
{
    internal class AddEditExtendedAttributeCommandLocalization
    {
        // for localization
    }

    public class AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequest<Result<TId>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TId Id { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TEntityId EntityId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public EntityExtendedAttributeType Type { get; set; }

        [Required(AllowEmptyStrings = false)]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Key { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public string? Text { get; set; }

        public decimal? Decimal { get; set; }

        public DateTime? DateTime { get; set; }

        public string? Json { get; set; }

        public string? ExternalId { get; set; }

        public string? Group { get; set; }

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }

    internal class AddEditExtendedAttributeCommandHandler<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequestHandler<AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute>, Result<TId>>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditExtendedAttributeCommandLocalization> _localizer;
        private readonly IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> _unitOfWork;

        public AddEditExtendedAttributeCommandHandler(
            IExtendedAttributeUnitOfWork<TId, TEntityId, TEntity> unitOfWork,
            IMapper mapper,
            IStringLocalizer<AddEditExtendedAttributeCommandLocalization> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<TId>> Handle(AddEditExtendedAttributeCommand<TId, TEntityId, TEntity, TExtendedAttribute> command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<TExtendedAttribute>().Entities.Where(x => !x.Id.Equals(command.Id) && x.EntityId!.Equals(command.EntityId))
                .AnyAsync(p => p.Key == command.Key, cancellationToken))
            {
                return await Result<TId>.FailAsync(_localizer["Extended Attribute with this Key already exists."]);
            }

            if (command.Id.Equals(default))
            {
                var extendedAttribute = _mapper.Map<TExtendedAttribute>(command);
                await _unitOfWork.Repository<TExtendedAttribute>().AddAsync(extendedAttribute);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllEntityExtendedAttributesCacheKey(typeof(TEntity).Name));

                // delete all caches related with added entity
                var cacheKeys = await _unitOfWork.Repository<TExtendedAttribute>().Entities.Select(x =>
                    ApplicationConstants.Cache.GetAllEntityExtendedAttributesByEntityIdCacheKey(
                        typeof(TEntity).Name, x.Entity.Id)).Distinct().ToArrayAsync(cancellationToken);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, cacheKeys);

                return await Result<TId>.SuccessAsync(extendedAttribute.Id, _localizer["Extended Attribute Saved"]);
            }
            else
            {
                var extendedAttribute = await _unitOfWork.Repository<TExtendedAttribute>().GetByIdAsync(command.Id);
                if (extendedAttribute != null)
                {
                    extendedAttribute.Key = command.Key;
                    extendedAttribute.EntityId = command.EntityId;
                    extendedAttribute.Type = command.Type;
                    extendedAttribute.Text = command.Text ?? extendedAttribute.Text;
                    extendedAttribute.Decimal = command.Decimal ?? extendedAttribute.Decimal;
                    extendedAttribute.DateTime = command.DateTime ?? extendedAttribute.DateTime;
                    extendedAttribute.Json = command.Json ?? extendedAttribute.Json;
                    extendedAttribute.ExternalId = command.ExternalId ?? extendedAttribute.ExternalId;
                    extendedAttribute.Group = command.Group ?? extendedAttribute.Group;
                    extendedAttribute.Description = command.Description ?? extendedAttribute.Description;
                    extendedAttribute.IsActive = command.IsActive;
                    await _unitOfWork.Repository<TExtendedAttribute>().UpdateAsync(extendedAttribute);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllEntityExtendedAttributesCacheKey(typeof(TEntity).Name));

                    // delete all caches related with updated entity
                    var cacheKeys = await _unitOfWork.Repository<TExtendedAttribute>().Entities.Select(x =>
                        ApplicationConstants.Cache.GetAllEntityExtendedAttributesByEntityIdCacheKey(
                            typeof(TEntity).Name, x.Entity.Id)).Distinct().ToArrayAsync(cancellationToken);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, cacheKeys);

                    return await Result<TId>.SuccessAsync(extendedAttribute.Id, _localizer["Extended Attribute Updated"]);
                }
                else
                {
                    return await Result<TId>.FailAsync(_localizer["Extended Attribute Not Found!"]);
                }
            }
        }
    }
}