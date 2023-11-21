using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.Identity;

[Route("api/v1/identity/roleClaims")]
public class RoleClaimsController : BaseApiController
{
    private readonly IRoleClaimService _roleClaimService;

    public RoleClaimsController(IRoleClaimService roleClaimService) => _roleClaimService = roleClaimService;

    /// <summary>
    ///     Get All Role Claims(e.g. Product Create Permission)
    /// </summary>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.RoleClaims.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<List<RoleClaimResponse>> roleClaims = await _roleClaimService.GetAllAsync();
        return Ok(roleClaims);
    }

    /// <summary>
    ///     Get All Role Claims By Id
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.RoleClaims.View)]
    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetAllByRoleId([FromRoute] string roleId)
    {
        Result<List<RoleClaimResponse>> response = await _roleClaimService.GetAllByRoleIdAsync(roleId);
        return Ok(response);
    }

    /// <summary>
    ///     Add a Role Claim
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK </returns>
    [Authorize(Policy = Permissions.RoleClaims.Create)]
    [HttpPost]
    public async Task<IActionResult> Post(RoleClaimRequest request)
    {
        Result<string> response = await _roleClaimService.SaveAsync(request);
        return Ok(response);
    }

    /// <summary>
    ///     Delete a Role Claim
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.RoleClaims.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Result<string> response = await _roleClaimService.DeleteAsync(id);
        return Ok(response);
    }
}
