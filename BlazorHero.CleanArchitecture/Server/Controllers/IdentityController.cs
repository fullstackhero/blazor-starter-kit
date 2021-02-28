using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Server.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService identity;
        private readonly ICurrentUserService currentUser;

        public IdentityController(
            IIdentityService identity,
            ICurrentUserService currentUser)
        {
            this.identity = identity;
            this.currentUser = currentUser;
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult> Register(
            RegisterRequest model)
            => await identity
                .RegisterAsync(model)
                .ToActionResult();

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<LoginResponse>> Login(
            LoginRequest model)
            => await identity
                .LoginAsync(model)
                .ToActionResult();

        [Authorize]
        [HttpPut(nameof(UpdateProfile))]
        public async Task<ActionResult> UpdateProfile(
            UpdateProfileRequest model)
            => await identity
                .UpdateProfileAsync(model, currentUser.UserId)
                .ToActionResult();

        [Authorize]
        [HttpPut(nameof(ChangePassword))]
        public async Task<ActionResult> ChangePassword(
            ChangePasswordRequest model)
        {
            return await identity
                           .ChangePasswordAsync(model, currentUser.UserId)
                           .ToActionResult();
        }
    }
}