using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.Identity
{
    [Authorize]
    [Route("api/identity/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[Authorize(Policy = Permissions.Users.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        //[Authorize(Policy = Permissions.Users.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }

        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetRolesAsync(string id)
        {
            var userRoles = await _userService.GetRolesAsync(id);
            return Ok(userRoles);
        }

        [Authorize(Policy = Permissions.Users.Edit)]
        [HttpPut("roles/{id}")]
        public async Task<IActionResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            return Ok(await _userService.UpdateRolesAsync(request));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _userService.RegisterAsync(request, origin));
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _userService.ConfirmEmailAsync(userId, code));
        }

        [HttpPost("toggle-status")]
        public async Task<IActionResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            return Ok(await _userService.ToggleUserStatusAsync(request));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _userService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _userService.ResetPasswordAsync(request));
        }
    }
}