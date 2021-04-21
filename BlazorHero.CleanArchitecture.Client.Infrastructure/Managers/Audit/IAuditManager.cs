using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.AuditService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Audit
{
    public interface IAuditManager : IManager
    {
        Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync();

        Task<string> DownloadFileAsync();
    }
}