using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.Identity;

[Authorize]
[Route("api/v1/identity/accounts")]
public class AccountsController : BaseApiController
{
    private readonly IAccountService _accountService;
    private readonly ICurrentUserService _currentUser;

    public AccountsController(IAccountService accountService, ICurrentUserService currentUser)
    {
        _accountService = accountService;
        _currentUser = currentUser;
    }

    /// <summary>
    ///     Update Profile
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPut("update-profile")]
    public async Task<ActionResult> UpdateProfile(UpdateProfileRequest model)
    {
        Result response = await _accountService.UpdateProfileAsync(model, _currentUser.UserId);
        return Ok(response);
    }

    /// <summary>
    ///     Change Password
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPut("change-password")]
    public async Task<ActionResult> ChangePassword(ChangePasswordRequest model)
    {
        Result response = await _accountService.ChangePasswordAsync(model, _currentUser.UserId);
        return Ok(response);
    }

    /// <summary>
    ///     Get Profile picture by Id
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Status 200 OK </returns>
    [HttpGet("{userId}/profile-picture")]
    [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
    public async Task<IActionResult> GetProfilePictureAsync(string userId) =>
        Ok(await _accountService.GetProfilePictureAsync(userId));

    /// <summary>
    ///     Update Profile Picture
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("{userId}/profile-picture")]
    public async Task<IActionResult> UpdateProfilePictureAsync(UpdateProfilePictureRequest request) =>
        Ok(await _accountService.UpdateProfilePictureAsync(request, _currentUser.UserId));
}
