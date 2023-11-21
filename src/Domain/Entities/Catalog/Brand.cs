using CleanBlazor.Domain.Abstractions;
using CleanBlazor.Domain.Primitives;

namespace CleanBlazor.Domain.Entities.Catalog;

public class Brand : AggregateRoot<int>, IAuditableEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Tax { get; set; }

    public string CreatedBy { get; }
    public DateTimeOffset CreatedOn { get; }
    public string LastModifiedBy { get; }
    public DateTimeOffset? LastModifiedOn { get; }

    public virtual ICollection<Product> Products { get; set; } =
        new HashSet<Product>();
}
