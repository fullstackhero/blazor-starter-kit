using BlazorHero.CleanArchitecture.Shared.Requests.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Identity.Authentication
{
    public interface IAuthenticationService : IClientService
    {
        Task<IResult> Login(TokenRequest model);

        Task<IResult> Logout();
    }
}