using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.Application.Features.Dashboards.Queries.GetData;

namespace BlazorHero.CleanArchitecture.Server.Controllers.v1
{
    [ApiController]
    public class DashboardController : BaseApiController<DashboardController>
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetDataAsync()
        {
            var result = await _mediator.Send(new GetDashboardDataQuery());
            return Ok(result);
        }
    }
}