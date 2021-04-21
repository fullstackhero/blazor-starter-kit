using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.TokenService.Interfaces.Requests;
using BlazorHero.CleanArchitecture.TokenService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Utils;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.TokenService.Interfaces
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}