using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers;

/// <summary>
///     Abstract BaseApi Controller Class
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private ISender _senderInstance;
    protected ISender Sender => _senderInstance ??= HttpContext.RequestServices.GetService<ISender>();
}
