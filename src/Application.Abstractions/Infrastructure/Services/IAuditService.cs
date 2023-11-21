using CleanBlazor.Contracts.Audit;
using CleanBlazor.Shared.Wrapper;

namespace CleanBlazor.Application.Abstractions.Infrastructure.Services;

public interface IAuditService
{
    Task<Result<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(string userId);

    Task<Result<string>> ExportToExcelAsync(
        string userId,
        string searchString = "",
        bool searchInOldValues = false,
        bool searchInNewValues = false);
}
