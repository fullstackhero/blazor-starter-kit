using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Infrastructure.Helpers;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using BlazorHero.CleanArchitecture.Shared.Constants.Role;
using BlazorHero.CleanArchitecture.Shared.Constants.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly IStringLocalizer<DatabaseSeeder> _localizer;
        private readonly BlazorHeroContext _db;
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly RoleManager<BlazorHeroRole> _roleManager;

        public DatabaseSeeder(
            UserManager<BlazorHeroUser> userManager,
            RoleManager<BlazorHeroRole> roleManager,
            BlazorHeroContext db,
            ILogger<DatabaseSeeder> logger,
            IStringLocalizer<DatabaseSeeder> localizer)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
            _localizer = localizer;
        }

        public void Initialize()
        {
            AddAdministrator();
            AddBasicUser();
            _db.SaveChanges();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new BlazorHeroRole(RoleConstants.AdministratorRole, _localizer["Administrator role with full permissions"]);
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    _logger.LogInformation(_localizer["Seeded Administrator Role."]);
                }
                //Check if User Exists
                var superUser = new BlazorHeroUser
                {
                    FirstName = "Mukesh",
                    LastName = "Murugan",
                    Email = "mukesh@blazorhero.com",
                    UserName = "mukesh",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
                }
                adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
                var RoleClaims = (await _roleManager.GetClaimsAsync(adminRole)).Select(c => c.Value).ToList();
                string[] permissions = ClaimsHelper.GetAllPermissionValues();
                var NewClaims = permissions.Except(RoleClaims);
                foreach (string claim in NewClaims)
                {
                    await _roleManager.AddClaimAsync(adminRole, new Claim(ApplicationClaimTypes.Permission, claim));
                }
                var DeprecatedClaims = RoleClaims.Except(permissions);
                var roles = _roleManager.Roles.ToList();
                foreach (string claim in DeprecatedClaims)
                {
                    foreach (var role in roles)
                    {
                        await _roleManager.RemoveClaimAsync(role, new Claim(ApplicationClaimTypes.Permission, claim));
                    }
                }
                _logger.LogInformation(_localizer["Seeded User with Administrator Role."]);
            }).GetAwaiter().GetResult();
        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var basicRole = new BlazorHeroRole(RoleConstants.BasicRole, _localizer["Basic role with default permissions"]);
                var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                    _logger.LogInformation(_localizer["Seeded Basic Role."]);
                }
                //Check if User Exists
                var basicUser = new BlazorHeroUser
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john@blazorhero.com",
                    UserName = "johndoe",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    CreatedOn = DateTime.Now,
                    IsActive = true
                };
                var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
                if (basicUserInDb == null)
                {
                    await _userManager.CreateAsync(basicUser, UserConstants.DefaultPassword);
                    await _userManager.AddToRoleAsync(basicUser, RoleConstants.BasicRole);
                    _logger.LogInformation(_localizer["Seeded User with Basic Role."]);
                }
            }).GetAwaiter().GetResult();
        }
    }
}