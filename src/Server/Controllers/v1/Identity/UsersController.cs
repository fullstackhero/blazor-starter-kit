using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace CleanBlazor.Server.Controllers.v1.Identity;

[Authorize]
[Route("api/v1/identity/users")]
public class UsersController : BaseApiController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    /// <summary>
    ///     Get Users Details
    /// </summary>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<List<UserResponse>> users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    ///     Get User By Id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Status 200 OK</returns>
    //[Authorize(Policy = Permissions.Users.View)]
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetById(string userId)
    {
        Result<UserResponse> user = await _userService.GetAsync(userId);
        return Ok(user);
    }

    /// <summary>
    ///     Get User Roles By Id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Users.View)]
    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetRolesAsync(string userId)
    {
        Result<UserRolesResponse> userRoles = await _userService.GetRolesAsync(userId);
        return Ok(userRoles);
    }

    /// <summary>
    ///     Update Roles for User
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Users.Edit)]
    [HttpPut("{userId}/roles")]
    public async Task<IActionResult> UpdateRolesAsync(UpdateUserRolesRequest request) =>
        Ok(await _userService.UpdateRolesAsync(request));

    /// <summary>
    ///     Register a User
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> RegisterAsync(RegisterRequest request)
    {
        StringValues origin = Request.Headers["origin"];
        return Ok(await _userService.RegisterAsync(request, origin));
    }

    /// <summary>
    ///     Confirm Email
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="code"></param>
    /// <returns>Status 200 OK</returns>
    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code) =>
        Ok(await _userService.ConfirmEmailAsync(userId, code));

    /// <summary>
    ///     Toggle User Status (Activate and Deactivate)
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("{userId}/toggle-status")]
    public async Task<IActionResult> ToggleUserStatusAsync(ToggleUserStatusRequest request) =>
        Ok(await _userService.ToggleUserStatusAsync(request));

    /// <summary>
    ///     Forgot Password
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        StringValues origin = Request.Headers["origin"];
        return Ok(await _userService.ForgotPasswordAsync(request, origin));
    }

    /// <summary>
    ///     Reset Password
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request) =>
        Ok(await _userService.ResetPasswordAsync(request));

    /// <summary>
    ///     Export to Excel
    /// </summary>
    /// <param name="searchString"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Users.Export)]
    [HttpGet("export")]
    public async Task<IActionResult> Export(string searchString = "")
    {
        var data = await _userService.ExportToExcelAsync(searchString);
        return Ok(data);
    }
}
