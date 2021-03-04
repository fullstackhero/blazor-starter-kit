using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Infrastructure;
using BlazorHero.CleanArchitecture.Shared.Requests.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers
{
    [Authorize(Roles =Constants.AdministratorRole)]
    [Route("api/identity/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            return Ok(roles);

        }
        [HttpPost]
        public async Task<IActionResult> Post(RoleRequest request)
        {
            var response = await _roleService.SaveAsync(request);
            return Ok(response);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _roleService.DeleteAsync(id);
            return Ok(response);

        }
    }
}