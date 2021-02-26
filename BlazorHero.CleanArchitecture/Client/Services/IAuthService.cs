using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Services
{
    public interface IAuthService
    {
        Task<Result> Register(RegisterRequest model);

        Task<Result> Login(LoginRequest model);

        Task Logout();
    }
}
