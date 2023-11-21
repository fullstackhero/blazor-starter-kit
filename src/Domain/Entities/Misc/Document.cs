using CleanBlazor.Domain.Abstractions;
using CleanBlazor.Domain.Primitives;

namespace CleanBlazor.Domain.Entities.Misc;

public class Document : AggregateRoot<int>, IAuditableEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsPublic { get; set; }
    public string Url { get; set; }
    public int DocumentTypeId { get; set; }
    public virtual DocumentType DocumentType { get; set; }
    public string CreatedBy { get; }
    public DateTimeOffset CreatedOn { get; }
    public string LastModifiedBy { get; }
    public DateTimeOffset? LastModifiedOn { get; }
}
