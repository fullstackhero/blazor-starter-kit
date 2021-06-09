using BlazorHero.CleanArchitecture.Application.Specifications.Base;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;

namespace BlazorHero.CleanArchitecture.Application.Specifications.Misc
{
    public class DocumentExtendedAttributeFilterSpecification : HeroSpecification<DocumentExtendedAttribute, int>
    {
        public DocumentExtendedAttributeFilterSpecification(string searchString, bool includeDocument, bool includeDocumentType)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Key.Contains(searchString)
                    || p.Text != null ? p.Text.Contains(searchString) : false
                    || p.Decimal != null ? p.Decimal.ToString().Contains(searchString) : false
                    || p.DateTime != null ? p.DateTime.ToString().Contains(searchString) : false
                    || p.Json != null ? p.Json.Contains(searchString) : false
                    || p.ExternalId != null ? p.ExternalId.Contains(searchString) : false
                    || p.Description != null ? p.Description.Contains(searchString) : false;
            }
            else
            {
                Criteria = p => true;
            }

            if (includeDocument)
            {
                Includes.Add(i => i.Entity);
            }

            if (includeDocumentType)
            {
                Includes.Add(i => i.Entity.DocumentType);
            }
        }
    }
}