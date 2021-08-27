#nullable enable
using System;
using BlazorHero.CleanArchitecture.Domain.Enums;

namespace BlazorHero.CleanArchitecture.Domain.Contracts
{
    public abstract class AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>
        : AuditableEntity<TId>, IEntityAuditableExtendedAttribute<TId, TEntityId, TEntity>
            where TEntity : IEntity<TEntityId>
    {
        /// <inheritdoc/>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TEntityId EntityId { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// Extended attribute's related entity
        /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public virtual TEntity Entity { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <inheritdoc/>
        public EntityExtendedAttributeType Type { get; set; }

        /// <inheritdoc/>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Key { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <inheritdoc/>
        public string? Text { get; set; }

        /// <inheritdoc/>
        public decimal? Decimal { get; set; }

        /// <inheritdoc/>
        public DateTime? DateTime { get; set; }

        /// <inheritdoc/>
        public string? Json { get; set; }

        /// <inheritdoc/>
        public string? ExternalId { get; set; }

        /// <inheritdoc/>
        public string? Group { get; set; }

        /// <inheritdoc/>
        public string? Description { get; set; }

        /// <inheritdoc/>
        public bool IsActive { get; set; } = true;
    }
}