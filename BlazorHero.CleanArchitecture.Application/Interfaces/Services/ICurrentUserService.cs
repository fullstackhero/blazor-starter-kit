using BlazorHero.CleanArchitecture.Application.Interfaces.Common;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}