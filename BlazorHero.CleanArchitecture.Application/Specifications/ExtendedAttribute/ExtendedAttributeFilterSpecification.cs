using System;
using BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.Export;
using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Contracts;

namespace BlazorHero.CleanArchitecture.Application.Specifications.ExtendedAttribute
{
    public class ExtendedAttributeFilterSpecification<TId, TEntityId, TEntity, TExtendedAttribute>
        : HeroSpecification<TExtendedAttribute>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TId>
            where TId : IEquatable<TId>
    {
        public ExtendedAttributeFilterSpecification(ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                Criteria = p =>
                    (p.EntityId.Equals(request.EntityId) || request.EntityId.Equals(default))
                    && (!request.OnlyCurrentGroup || request.CurrentGroup.Equals(p.Group))
                    //&& (p.Key != null ? p.Key.Contains(request.SearchString) : false
                    //    || p.Text != null ? p.Text.Contains(request.SearchString) : false
                    //    || p.Decimal != null ? p.Decimal.ToString().Contains(request.SearchString) : false
                    //    || p.DateTime != null ? p.DateTime.ToString().Contains(request.SearchString) : false
                    //    || p.Json != null ? p.Json.Contains(request.SearchString) : false
                    //    || p.ExternalId != null ? p.ExternalId.Contains(request.SearchString) : false
                    //    || p.Group != null ? p.Group.Contains(request.SearchString) : false
                    //    || p.Description != null ? p.Description.Contains(request.SearchString) : false)
                    ;
            }
            else
            {
                Criteria = p =>
                    (p.EntityId.Equals(request.EntityId) || request.EntityId.Equals(default))
                    && (!request.OnlyCurrentGroup || request.CurrentGroup.Equals(p.Group));
            }

            if (request.IncludeEntity)
            {
                Includes.Add(i => i.Entity);
            }
        }
    }
}