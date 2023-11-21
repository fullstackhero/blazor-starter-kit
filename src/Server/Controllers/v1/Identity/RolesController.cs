using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.Identity;

[Route("api/v1/identity/roles")]
public class RolesController : BaseApiController
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService) => _roleService = roleService;

    /// <summary>
    ///     Get All Roles (basic, admin etc.)
    /// </summary>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Roles.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<List<RoleResponse>> roles = await _roleService.GetAllAsync();
        return Ok(roles);
    }

    /// <summary>
    ///     Add a Role
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Roles.Create)]
    [HttpPost]
    public async Task<IActionResult> Post(RoleRequest request)
    {
        Result<string> response = await _roleService.SaveAsync(request);
        return Ok(response);
    }

    /// <summary>
    ///     Delete a Role
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.Roles.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        Result<string> response = await _roleService.DeleteAsync(id);
        return Ok(response);
    }

    /// <summary>
    ///     Get Permissions By Role Id
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns>Status 200 Ok</returns>
    [Authorize(Policy = Permissions.RoleClaims.View)]
    [HttpGet("{roleId}/permissions")]
    public async Task<IActionResult> GetPermissionsByRoleId([FromRoute] string roleId)
    {
        Result<PermissionResponse> response = await _roleService.GetAllPermissionsAsync(roleId);
        return Ok(response);
    }

    /// <summary>
    ///     Edit role permissions for Role Id
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = Permissions.RoleClaims.Edit)]
    [HttpPut("{roleId}/permissions")]
    public async Task<IActionResult> Update(PermissionRequest model)
    {
        Result<string> response = await _roleService.UpdatePermissionsAsync(model);
        return Ok(response);
    }
}
