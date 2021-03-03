using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers.Identity
{
    [Route("api/identity/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService currentUserService;

        public TokenController(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            this.currentUserService = currentUserService;
        }  

        [HttpPost]
        public async Task<ActionResult> Get(TokenRequest model)
        {
            var response = await _identityService.LoginAsync(model);
            return Ok(response);
        }
    }
}
