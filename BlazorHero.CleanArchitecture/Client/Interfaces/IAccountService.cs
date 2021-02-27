using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Interfaces
{
    public interface IAccountService
    {
        Task ChangePasswordAsync(ChangePasswordRequest model);
        Task<Result> UpdateProfiledAsync(ChangePasswordRequest model, string userId);
    }
}
