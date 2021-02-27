using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user == null)
            {
                throw new InvalidOperationException("This request does not have an authenticated user.");
            }

            this.UserId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string UserId { get; }
    }
}