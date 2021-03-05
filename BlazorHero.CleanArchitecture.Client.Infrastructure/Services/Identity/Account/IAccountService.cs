using BlazorHero.CleanArchitecture.Application.Shared.Identity;
using BlazorHero.CleanArchitecture.Shared.Requests.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Identity.Account
{
    public interface IAccountService : IClientService
    {
        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model);

        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model);
    }
}