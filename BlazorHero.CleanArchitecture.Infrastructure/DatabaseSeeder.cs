using BlazorHero.CleanArchitecture.Application.Helpers;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Application.Models.Identity;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using BlazorHero.CleanArchitecture.Shared.Constants.Role;
using BlazorHero.CleanArchitecture.Shared.Constants.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly BlazorHeroContext _db;
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(UserManager<BlazorHeroUser> userManager, RoleManager<IdentityRole> roleManager, BlazorHeroContext db, ILogger<DatabaseSeeder> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;
        }

        public void Initialize()
        {
            AddCustomerPermissionClaims();
            AddAdministrator();
            AddBasicUser();
            _db.SaveChanges();
        }

        private void AddCustomerPermissionClaims()
        {
            Task.Run(async () =>
            {
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstant.AdministratorRole);
                if (adminRoleInDb != null)
                {
                    await _roleManager.AddCustomPermissionClaim(adminRoleInDb, "Permissions.Communication.Chat");                    
                }               
            }).GetAwaiter().GetResult();
        }

        private void AddAdministrator()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new IdentityRole(RoleConstant.AdministratorRole);
                var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstant.AdministratorRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    _logger.LogInformation("Seeded Administrator Role.");
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
                    await _userManager.CreateAsync(superUser, UserConstant.DefaultPassword);
                    var result = await _userManager.AddToRoleAsync(superUser, RoleConstant.AdministratorRole);
                    if (result.Succeeded)
                    {
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Users);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Roles);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Products);
                        await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Brands);
                       
                    }
                    _logger.LogInformation("Seeded User with Administrator Role.");
                }
            }).GetAwaiter().GetResult();
        }

        private void AddBasicUser()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var basicRole = new IdentityRole(RoleConstant.BasicRole);
                var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstant.BasicRole);
                if (basicRoleInDb == null)
                {
                    await _roleManager.CreateAsync(basicRole);
                    _logger.LogInformation("Seeded Basic Role.");
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
                    await _userManager.CreateAsync(basicUser, UserConstant.DefaultPassword);
                    await _userManager.AddToRoleAsync(basicUser, RoleConstant.BasicRole);
                    _logger.LogInformation("Seeded User with Basic Role.");
                }
            }).GetAwaiter().GetResult();
        }
    }
}