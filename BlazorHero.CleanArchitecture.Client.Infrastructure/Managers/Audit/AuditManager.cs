using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.AuditService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Audit
{
    public class AuditManager : IAuditManager
    {
        private readonly HttpClient _httpClient;

        public AuditManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync()
        {
            var response = await _httpClient.GetAsync(Routes.AuditEndpoints.GetCurrentUserTrails);
            var data = await response.ToResult<IEnumerable<AuditResponse>>();
            return data;
        }

        public async Task<string> DownloadFileAsync()
        {
            var response = await _httpClient.GetAsync(Routes.AuditEndpoints.DownloadFile);
            var data = await response.Content.ReadAsStringAsync();
            return data;
        }
    }
}