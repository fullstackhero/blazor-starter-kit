using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanBlazor.Server.Controllers.v1.Identity;

[Route("api/v1/identity/token")]
[AllowAnonymous]
public class TokenController : BaseApiController
{
    private readonly ITokenService _identityService;

    public TokenController(ITokenService identityService) =>
        _identityService = identityService;

    /// <summary>
    ///     Get Token (Email, Password)
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost]
    public async Task<ActionResult> Get(TokenRequest model)
    {
        Result<TokenResponse> response = await _identityService.LoginAsync(model);
        return Ok(response);
    }

    /// <summary>
    ///     Refresh Token
    /// </summary>
    /// <param name="model"></param>
    /// <returns>Status 200 OK</returns>
    [HttpPost("refresh")]
    public async Task<ActionResult> Refresh([FromBody] RefreshTokenRequest model)
    {
        Result<TokenResponse> response = await _identityService.GetRefreshTokenAsync(model);
        return Ok(response);
    }
}
