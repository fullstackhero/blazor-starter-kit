using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;

namespace BlazorHero.CleanArchitecture.Server.Controllers.Utilities
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuditsController : ControllerBase
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAuditService _auditService;

        public AuditsController(ICurrentUserService currentUserService, IAuditService auditService)
        {
            _currentUserService = currentUserService;
            _auditService = auditService;
        }
        /// <summary>
        /// Get Current User Trails
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.AuditTrails.View)]
        [HttpGet]
        public async Task<IActionResult> GetUserTrailsAsync()
        {
            return Ok(await _auditService.GetCurrentUserTrailsAsync(_currentUserService.UserId));
        }
        /// <summary>
        /// Export Audit Train to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="searchInOldValues"></param>
        /// <param name="searchInNewValues"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.AuditTrails.View)]
        [HttpGet("export")]
        public async Task<IActionResult> ExportExcel(string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false)
        {
            var data = await _auditService.ExportToExcelAsync(_currentUserService.UserId, searchString, searchInOldValues, searchInNewValues);
            return Ok(data);
        }
    }
}