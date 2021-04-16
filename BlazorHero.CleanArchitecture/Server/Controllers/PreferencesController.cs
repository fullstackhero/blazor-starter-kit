using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Infrastructure.Managers.Preferences;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorHero.CleanArchitecture.Server.Controllers
{
    [Route("api/preferences")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        private readonly IServerPreferenceManager _serverPreferenceManager;

        public PreferencesController(IServerPreferenceManager serverPreferenceManager)
        {
            _serverPreferenceManager = serverPreferenceManager;
        }

        [Authorize(Policy = Permissions.Preferences.ChangeLanguage)]
        [HttpPost("language/change")]
        public async Task ChangeLanguage(string languageCode)
        {
            await _serverPreferenceManager.ChangeLanguageAsync(languageCode);
        }
    }
}
