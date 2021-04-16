using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorHero.CleanArchitecture.Server.Controllers
{
    [Route("api/preferences")]
    [ApiController]
    public class PreferencesController : ControllerBase
    {
        //private readonly IServerPreferenceManager _serverPreferenceManager;
        private readonly IRoleService _roleService;

        public PreferencesController(
            //IServerPreferenceManager serverPreferenceManager,
            IRoleService roleService)
        {
            //_serverPreferenceManager = serverPreferenceManager;
            _roleService = roleService;
        }

        //TODO - add actions
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //var response = await _identityService.LoginAsync(model);
            return Ok(_roleService.Test());
        }
    }
}
