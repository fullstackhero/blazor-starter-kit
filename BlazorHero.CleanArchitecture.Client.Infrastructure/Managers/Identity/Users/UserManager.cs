using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Identity.Users
{
    public class UserManager : IUserManager
    {
        private readonly HttpClient _httpClient;

        public UserManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<List<UserResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.UserEndpoint.GetAll);
            return await response.ToResult<List<UserResponse>>();
        }

        public async Task<IResult> RegisterUserAsync(RegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(Routes.UserEndpoint.Register, request);
                return await response.ToResult();
            }
            catch (System.Exception ex)
            {

                throw;
            }
            
        }
    }
}