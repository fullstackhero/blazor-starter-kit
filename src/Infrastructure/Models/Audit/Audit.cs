using CleanBlazor.Domain.Abstractions;

namespace CleanBlazor.Infrastructure.Models.Audit;

public class Audit : IEntity<int>
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Type { get; set; }
    public string TableName { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public string AffectedColumns { get; set; }
    public string PrimaryKey { get; set; }
}
