using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services.Account
{
    public interface IAccountService
    {
        Task ChangePasswordAsync(ChangePasswordRequest model);

        Task UpdateProfiledAsync(UpdateProfileRequest model);
    }
}