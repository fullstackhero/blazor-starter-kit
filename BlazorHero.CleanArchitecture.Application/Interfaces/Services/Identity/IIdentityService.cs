using BlazorHero.CleanArchitecture.Application.Interfaces.Common;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity
{
    public interface IIdentityService : IService
    {
        Task<Result> RegisterAsync(RegisterRequest model);

        Task<Result<LoginResponse>> LoginAsync(LoginRequest model);

        Task<Result> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<Result> ChangePasswordAsync(ChangePasswordRequest model, string userId);
    }
}