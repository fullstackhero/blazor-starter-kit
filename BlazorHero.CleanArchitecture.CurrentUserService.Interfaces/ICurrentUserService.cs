using BlazorHero.CleanArchitecture.Utils;

namespace BlazorHero.CleanArchitecture.CurrentUserService.Interfaces
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}