using BlazorHero.CleanArchitecture.Application.Configurations;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static BlazorHero.CleanArchitecture.Application.Configurations.Constants;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private const string InvalidErrorMessage = "Invalid email or password.";

        private readonly UserManager<BlazorHeroUser> userManager;
        private readonly AppConfiguration appConfig;
        public IdentityService(
            UserManager<BlazorHeroUser> userManager, IOptions<AppConfiguration> appConfig)
        {
            this.userManager = userManager;
            this.appConfig = appConfig.Value;
        }

        public async Task<Result> RegisterAsync(RegisterRequest model)
        {
            var user = new BlazorHeroUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email
            };

            var identityResult = await this.userManager.CreateAsync(user, model.Password);

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }

        public async Task<Result<LoginResponse>> LoginAsync(LoginRequest model)
        {
            try
            {
                var user = await this.userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return InvalidErrorMessage;
                }

                var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordValid)
                {
                    return InvalidErrorMessage;
                }

                var token = await GenerateJwtAsync(user);

                return new LoginResponse { Token = token };
            }
            catch
            {

                throw;
            }
            
        }
        private async Task<string> GenerateJwtAsync(BlazorHeroUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName)
            };

            var isAdministrator = await this.userManager.IsInRoleAsync(user, AdministratorRole);

            if (isAdministrator)
            {
                claims.Add(new Claim(ClaimTypes.Role, AdministratorRole));
            }

            var secret = Encoding.UTF8.GetBytes(this.appConfig.Secret);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(secret),
                    SecurityAlgorithms.HmacSha256));

            var tokenHandler = new JwtSecurityTokenHandler();
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
        public async Task<Result> ChangeSettingsAsync(
            ChangeSettingsRequest model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return InvalidErrorMessage;
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;

            var identityResult = await this.userManager.UpdateAsync(user);

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }

        public async Task<Result> ChangePasswordAsync(
            ChangePasswordRequest model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return InvalidErrorMessage;
            }

            var identityResult = await this.userManager.ChangePasswordAsync(
                user,
                model.Password,
                model.NewPassword);

            var errors = identityResult.Errors.Select(e => e.Description);

            return identityResult.Succeeded
                ? Result.Success
                : Result.Failure(errors);
        }
    }
}
