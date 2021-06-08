using System.Collections.Generic;
using BlazorHero.CleanArchitecture.Domain.Contracts;
using BlazorHero.CleanArchitecture.Domain.Entities.ExtendedAttributes;

namespace BlazorHero.CleanArchitecture.Domain.Entities.Misc
{
    public class Document : AuditableEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; } = false;
        public string URL { get; set; }
        public int DocumentTypeId { get; set; }
        public virtual DocumentType DocumentType { get; set; }
        public virtual ICollection<DocumentExtendedAttribute> DocumentExtendedAttributes { get; set; }

        public Document()
        {
            DocumentExtendedAttributes = new HashSet<DocumentExtendedAttribute>();
        }
    }
}