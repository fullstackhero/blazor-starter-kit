using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<IResult> Login(TokenRequest model);

        Task<IResult> Logout();
    }
}