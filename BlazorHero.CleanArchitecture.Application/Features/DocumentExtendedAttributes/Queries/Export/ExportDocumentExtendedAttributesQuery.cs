using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Extensions;
using BlazorHero.CleanArchitecture.Application.Interfaces.Repositories;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Specifications.Misc;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;
using BlazorHero.CleanArchitecture.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Application.Features.DocumentExtendedAttributes.Queries.Export
{
    public class ExportDocumentExtendedAttributesQuery : IRequest<string>
    {
        public string SearchString { get; set; }
        public bool IncludeDocument { get; set; }
        public bool IncludeDocumentType { get; set; }

        public ExportDocumentExtendedAttributesQuery(string searchString = "", bool includeDocument = false, bool includeDocumentType = false)
        {
            SearchString = searchString;
            IncludeDocument = includeDocument;
            IncludeDocumentType = includeDocumentType;
        }
    }

    internal class ExportDocumentExtendedAttributesQueryHandler : IRequestHandler<ExportDocumentExtendedAttributesQuery, string>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportDocumentExtendedAttributesQueryHandler> _localizer;

        public ExportDocumentExtendedAttributesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportDocumentExtendedAttributesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<string> Handle(ExportDocumentExtendedAttributesQuery request, CancellationToken cancellationToken)
        {
            var documentExtendedAttributeFilterSpec = new DocumentExtendedAttributeFilterSpecification(request.SearchString, request.IncludeDocument, request.IncludeDocumentType);
            var documentExtendedAttributes = await _unitOfWork.Repository<DocumentExtendedAttribute>().Entities
                .Specify(documentExtendedAttributeFilterSpec)
                .ToListAsync(cancellationToken);

            var mappers = new Dictionary<string, Func<DocumentExtendedAttribute, object>>
            {
                {_localizer["Id"], item => item.Id},
                {_localizer["DocumentId"], item => item.EntityId},
                {_localizer["Type"], item => item.Type},
                {_localizer["Key"], item => item.Key},
                {
                    _localizer["Value"], item => item.Type switch
                    {
                        EntityExtendedAttributeType.Decimal => item.Decimal,
                        EntityExtendedAttributeType.Text => item.Text,
                        EntityExtendedAttributeType.DateTime => item.DateTime,
                        EntityExtendedAttributeType.Json => item.Json,
                        _ => throw new ArgumentOutOfRangeException(nameof(item.Type), _localizer["TODO"])
                    }
                },
                {_localizer["ExternalId"], item => item.ExternalId},
                {_localizer["Description"], item => item.Description},
                {_localizer["IsActive"], item => item.IsActive}
            };

            if (request.IncludeDocument)
            {
                mappers.Add(_localizer["DocumentTitle"], item => item.Entity.Title);
                mappers.Add(_localizer["DocumentDescription"], item => item.Entity.Description);
                mappers.Add(_localizer["DocumentIsPublic"], item => item.Entity.IsPublic);
                mappers.Add(_localizer["DocumentURL"], item => item.Entity.URL);
            }

            if (request.IncludeDocumentType)
            {
                mappers.Add(_localizer["DocumentTypeId"], item => item.Entity.DocumentTypeId);
                mappers.Add(_localizer["DocumentTypeName"], item => item.Entity.DocumentType.Name);
                mappers.Add(_localizer["DocumentTypeDescription"], item => item.Entity.DocumentType.Description);
            }

            var data = await _excelService.ExportAsync(documentExtendedAttributes, mappers: mappers,
                sheetName: _localizer["Document Extended Attributes"]);

            return data;
        }
    }
}