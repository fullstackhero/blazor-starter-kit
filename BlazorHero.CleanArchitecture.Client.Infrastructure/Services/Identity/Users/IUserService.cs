using BlazorHero.CleanArchitecture.Shared.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Services.Identity.Users
{
    public interface IUserService : IClientService
    {
        Task<IResult<GetAllUsersReponse>> GetAllAsync();
    }
}