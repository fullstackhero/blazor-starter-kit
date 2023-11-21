using CleanBlazor.Application.Specifications.Base;
using CleanBlazor.Infrastructure.Models.Audit;

namespace CleanBlazor.Infrastructure.Specifications;

public class AuditFilterSpecification : BaseSpecification<Audit>
{
    public AuditFilterSpecification(string userId, string searchString, bool searchInOldValues, bool searchInNewValues)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            Criteria = p =>
                (p.TableName.Contains(searchString) || (searchInOldValues && p.OldValues.Contains(searchString)) ||
                 (searchInNewValues && p.NewValues.Contains(searchString))) && p.UserId == userId;
        }
        else
        {
            Criteria = p => p.UserId == userId;
        }
    }
}
