using CleanBlazor.Application.Features.Dashboards.Queries;
using CleanBlazor.Contracts.Dashboard;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1;

public class DashboardController : BaseApiController
{
    /// <summary>
    ///     Get Dashboard Data
    /// </summary>
    /// <returns>Status 200 OK </returns>
    [Authorize(Policy = Permissions.Dashboards.View)]
    [HttpGet]
    public async Task<IActionResult> GetDataAsync()
    {
        Result<DashboardDataResponse> result = await Sender.Send(new GetDashboardDataQuery());
        return Ok(result);
    }
}
