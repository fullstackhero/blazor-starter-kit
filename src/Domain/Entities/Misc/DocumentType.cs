using CleanBlazor.Domain.Abstractions;
using CleanBlazor.Domain.Primitives;

namespace CleanBlazor.Domain.Entities.Misc;

public class DocumentType : AggregateRoot<int>, IAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string CreatedBy { get; }
    public DateTimeOffset CreatedOn { get; }
    public string LastModifiedBy { get; }
    public DateTimeOffset? LastModifiedOn { get; }

    public virtual ICollection<Document> Documents { get; set; } =
        new HashSet<Document>();
}
