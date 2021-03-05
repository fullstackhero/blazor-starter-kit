using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Requests.Roles;
using BlazorHero.CleanArchitecture.Shared.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Identity.Roles
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;

        public RoleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> DeleteAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.RolesEndpoint.Delete}/{id}");
            return await response.ToResult<string>();
        }

        public async Task<IResult<GetAllRolesResponse>> GetRolesAsync()
        {
            var response = await _httpClient.GetAsync(Routes.RolesEndpoint.GetAll);
            return await response.ToResult<GetAllRolesResponse>();
        }

        public async Task<IResult<string>> SaveAsync(RoleRequest role)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.RolesEndpoint.Save,role);
            return await response.ToResult<string>();
        }
    }
}
