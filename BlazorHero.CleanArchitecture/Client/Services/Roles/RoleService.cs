using BlazorHero.CleanArchitecture.Application.Requests.Roles;
using BlazorHero.CleanArchitecture.Application.Responses.Roles;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using BlazorHero.CleanArchitecture.Client.Interfaces;
using BlazorHero.CleanArchitecture.Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services.Roles
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;

        public RoleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult> DeleteAsync(string id)
        {
            var reponse = await _httpClient.DeleteAsync($"{Routes.RolesEndpoint.Delete}/{id}");
            return Result.Success();
        }

        public async Task<Result<GetAllRolesResponse>> GetRolesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<Result<GetAllRolesResponse>>(Routes.RolesEndpoint.GetAll);
            return response;
        }

        public async Task<IResult> SaveAsync(RoleRequest role)
        {
            await _httpClient.PostAsJsonAsync(Routes.RolesEndpoint.Save,role);
            return Result.Success();
        }
    }
}
