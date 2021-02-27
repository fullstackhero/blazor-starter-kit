using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            _db.Database.Migrate();
            AddSuperUser();
            _db.SaveChanges();
        }

        private void AddSuperUser()
        {
            Task.Run(async () =>
            {
                //Check if Role Exists
                var adminRole = new IdentityRole(Constants.SuperAdminRole);
                var adminRoleInDb = await _roleManager.FindByNameAsync(Constants.SuperAdminRole);
                if (adminRoleInDb == null)
                {
                    await _roleManager.CreateAsync(adminRole);
                    _logger.LogInformation("Seeded Role.");
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
                    CreatedOn = DateTime.Now
                };
                var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
                if (superUserInDb == null)
                {
                    await _userManager.CreateAsync(superUser, Constants.DefaultPassword);
                    await _userManager.AddToRoleAsync(superUser, Constants.SuperAdminRole);
                    _logger.LogInformation("Seeded Super User.");
                }
                
            }).GetAwaiter().GetResult();
        }
    }
}