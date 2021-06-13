using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Specifications.ExtendedAttribute;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.ExtendedAttributes.Queries.Export
{
    internal class ExportExtendedAttributesQueryLocalization
    {
        // for localization
    }

    public class ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequest<string>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TEntityId>
            where TId : IEquatable<TId>
    {
        public string SearchString { get; set; }
        public TEntityId EntityId { get; set; }
        public bool IncludeEntity { get; set; }

        public ExportExtendedAttributesQuery(string searchString = "", TEntityId entityId = default, bool includeEntity = false)
        {
            SearchString = searchString;
            EntityId = entityId;
            IncludeEntity = includeEntity;
        }
    }

    internal class ExportExtendedAttributesQueryHandler<TId, TEntityId, TEntity, TExtendedAttribute>
        : IRequestHandler<ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>, string>
            where TEntity : AuditableEntity<TEntityId>, IEntityWithExtendedAttributes<TExtendedAttribute>, IEntity<TEntityId>
            where TExtendedAttribute : AuditableEntityExtendedAttribute<TId, TEntityId, TEntity>, IEntity<TEntityId>
            where TId : IEquatable<TId>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<TId> _unitOfWork;
        private readonly IStringLocalizer<ExportExtendedAttributesQueryLocalization> _localizer;

        public ExportExtendedAttributesQueryHandler(IExcelService excelService
            , IUnitOfWork<TId> unitOfWork
            , IStringLocalizer<ExportExtendedAttributesQueryLocalization> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<string> Handle(ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute> request, CancellationToken cancellationToken)
        {
            var extendedAttributeFilterSpec = new ExtendedAttributeFilterSpecification<TId, TEntityId, TEntity, TExtendedAttribute>(request.SearchString, request.EntityId, request.IncludeEntity);
            var extendedAttributes = await _unitOfWork.Repository<TExtendedAttribute>().Entities
                .Specify(extendedAttributeFilterSpec)
                .ToListAsync(cancellationToken);

            var mappers = new Dictionary<string, Func<TExtendedAttribute, object>>
            {
                {_localizer["Id"], item => item.Id},
                {_localizer["EntityId"], item => item.EntityId},
                {_localizer["Type"], item => item.Type},
                {_localizer["Key"], item => item.Key},
                {
                    _localizer["Value"], item => item.Type switch
                    {
                        EntityExtendedAttributeType.Decimal => item.Decimal,
                        EntityExtendedAttributeType.Text => item.Text,
                        EntityExtendedAttributeType.DateTime => item.DateTime != null ? DateTime.SpecifyKind((DateTime)item.DateTime, DateTimeKind.Utc).ToLocalTime().ToString("G", CultureInfo.CurrentCulture) : string.Empty,
                        EntityExtendedAttributeType.Json => item.Json,
                        _ => throw new ArgumentOutOfRangeException(nameof(item.Type), _localizer["Type should be valid"])
                    }
                },
                {_localizer["ExternalId"], item => item.ExternalId},
                {_localizer["Group"], item => item.Group},
                {_localizer["Description"], item => item.Description},
                {_localizer["IsActive"], item => item.IsActive}
            };

            if (request.IncludeEntity)
            {
                mappers.Add(_localizer["EntityCreatedBy"], item => item.Entity.CreatedBy);
                mappers.Add(_localizer["EntityCreatedOn"], item => item.Entity.CreatedOn);
                mappers.Add(_localizer["EntityLastModifiedBy"], item => item.Entity.LastModifiedBy);
                mappers.Add(_localizer["EntityLastModifiedOn"], item => item.Entity.LastModifiedOn);
            }

            var data = await _excelService.ExportAsync(extendedAttributes, mappers: mappers,
                sheetName: string.Format(_localizer["{0} Extended Attributes"], typeof(TEntity).Name));

            return data;
        }
    }
}