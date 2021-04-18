using BlazorHero.CleanArchitecture.Shared.Settings;

namespace BlazorHero.CleanArchitecture.Infrastructure.Settings
{
    public record ServerPreference : IPreference
    {
        //private readonly ICurrentUserService _currentUserService;

        public ServerPreference(
            //ICurrentUserService currentUserService
            )
        {
            //_currentUserService = currentUserService;
        }

        //TODO - add server preferences
    }
}
