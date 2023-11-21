using CleanBlazor.Domain.Abstractions;
using CleanBlazor.Domain.Primitives;

namespace CleanBlazor.Domain.Entities.Catalog;

public class Product : AggregateRoot<int>, IAuditableEntity
{
    public string Name { get; set; }
    public string Barcode { get; set; }
    public string ImageDataUrl { get; set; }
    public string Description { get; set; }
    public decimal Rate { get; set; }
    public int BrandId { get; set; }
    public virtual Brand Brand { get; set; }
    public string CreatedBy { get; }
    public DateTimeOffset CreatedOn { get; }
    public string LastModifiedBy { get; }
    public DateTimeOffset? LastModifiedOn { get; }
}
