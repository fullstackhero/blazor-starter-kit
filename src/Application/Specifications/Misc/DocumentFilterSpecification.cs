using CleanBlazor.Application.Specifications.Base;
using CleanBlazor.Domain.Entities.Misc;

namespace CleanBlazor.Application.Specifications.Misc;

public class DocumentFilterSpecification : BaseSpecification<Document>
{
    public DocumentFilterSpecification(string searchString, string userId)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            Criteria = p =>
                (p.Title.Contains(searchString) || p.Description.Contains(searchString)) &&
                (p.IsPublic || (!p.IsPublic && p.CreatedBy == userId));
        }
        else
        {
            Criteria = p => p.IsPublic || (!p.IsPublic && p.CreatedBy == userId);
        }
    }
}
