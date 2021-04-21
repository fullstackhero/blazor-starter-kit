using BlazorHero.CleanArchitecture.Client.Infrastructure.Extensions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.AccountService.Interfaces.Requests;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Managers.Identity.Account
{
    public class AccountManager : IAccountManager
    {
        private readonly HttpClient _httpClient;

        public AccountManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.ChangePassword, model);
            return await response.ToResult();
        }

        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model)
        {
            var response = await _httpClient.PutAsJsonAsync(Routes.AccountEndpoints.UpdateProfile, model);
            return await response.ToResult();
        }

        public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        {
            var response = await _httpClient.GetAsync(Routes.AccountEndpoints.GetProfilePicture(userId));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.AccountEndpoints.GetProfilePicture(userId), request);
            return await response.ToResult<string>();
        }
    }
}