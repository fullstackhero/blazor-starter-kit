using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.AuditService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.AuditService.Interfaces
{
    public interface IAuditService
    {
        Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(string userId);

        Task<string> ExportToExcelAsync(string userId);
    }
}