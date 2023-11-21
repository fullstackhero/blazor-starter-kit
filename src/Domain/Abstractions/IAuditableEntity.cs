namespace CleanBlazor.Domain.Abstractions;

public interface IAuditableEntity : IEntity
{
    string CreatedBy { get; }
    DateTimeOffset CreatedOn { get; }
    string LastModifiedBy { get; }
    DateTimeOffset? LastModifiedOn { get; }
}
