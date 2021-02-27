using BlazorHero.CleanArchitecture.Application.Interfaces.Common;

namespace BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}