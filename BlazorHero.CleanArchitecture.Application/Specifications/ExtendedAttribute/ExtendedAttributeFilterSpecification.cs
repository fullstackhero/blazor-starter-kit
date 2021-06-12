using System;
using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Application.Specifications.ExtendedAttribute
{
    public class ExtendedAttributeFilterSpecification<TId, TEntityId, TEntity, TExtendedAttribute>
        : HeroSpecification<TExtendedAttribute, TEntityId>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TEntityId>
            where TId : IEquatable<TId>
    {
        public ExtendedAttributeFilterSpecification(string searchString, TEntityId entityId, bool includeEntity)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.EntityId.Equals(entityId) || entityId.Equals(default))
                    && (p.Key.Contains(searchString)
                        || p.Text != null ? p.Text.Contains(searchString) : false
                        || p.Decimal != null ? p.Decimal.ToString().Contains(searchString) : false
                        || p.DateTime != null ? p.DateTime.ToString().Contains(searchString) : false
                        || p.Json != null ? p.Json.Contains(searchString) : false
                        || p.ExternalId != null ? p.ExternalId.Contains(searchString) : false
                        || p.Description != null ? p.Description.Contains(searchString) : false);
            }
            else
            {
                Criteria = p => p.EntityId.Equals(entityId) || entityId.Equals(default);
            }

            if (includeEntity)
            {
                Includes.Add(i => i.Entity);
            }
        }
    }
}