using BlazorHero.CleanArchitecture.Application.Interfaces.Common;
using BlazorHero.CleanArchitecture.Shared.Requests.Identity;
using BlazorHero.CleanArchitecture.Shared.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);
    }
}