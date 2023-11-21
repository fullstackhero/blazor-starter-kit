using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.Utilities;

public class PreferencesController : BaseApiController
{
    private readonly ServerPreferenceManager _serverPreferenceManager;

    public PreferencesController(ServerPreferenceManager serverPreferenceManager) =>
        _serverPreferenceManager = serverPreferenceManager;

    /// <summary>
    ///     Change Language Preference
    /// </summary>
    /// <param name="languageCode"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Preferences.ChangeLanguage)]
    [HttpPost("changeLanguage")]
    public async Task<IActionResult> ChangeLanguageAsync(string languageCode)
    {
        Result result = await _serverPreferenceManager.ChangeLanguageAsync(languageCode);
        return Ok(result);
    }
}
