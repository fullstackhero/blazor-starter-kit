using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.Utilities;

[Authorize]
public class AuditsController : BaseApiController
{
    private readonly IAuditService _auditService;
    private readonly ICurrentUserService _currentUserService;

    public AuditsController(ICurrentUserService currentUserService, IAuditService auditService)
    {
        _currentUserService = currentUserService;
        _auditService = auditService;
    }

    /// <summary>
    ///     Get Current User Audit Trails
    /// </summary>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.AuditTrails.View)]
    [HttpGet]
    public async Task<IActionResult> GetUserTrailsAsync() =>
        Ok(await _auditService.GetCurrentUserTrailsAsync(_currentUserService.UserId));

    /// <summary>
    ///     Search Audit Trails and Export to Excel
    /// </summary>
    /// <param name="searchString"></param>
    /// <param name="searchInOldValues"></param>
    /// <param name="searchInNewValues"></param>
    /// <returns>Status 200 OK</returns>
    [Authorize(Policy = Permissions.AuditTrails.Export)]
    [HttpGet("export")]
    public async Task<IActionResult> ExportExcel(
        string searchString = "",
        bool searchInOldValues = false,
        bool searchInNewValues = false)
    {
        Result<string> data = await _auditService.ExportToExcelAsync(_currentUserService.UserId,
            searchString,
            searchInOldValues,
            searchInNewValues);
        return Ok(data);
    }
}
