using BlazorHero.CleanArchitecture.Application.Interfaces.Services;
using BlazorHero.CleanArchitecture.Infrastructure.Contexts;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure
{
    public class DatabaseSeeder : IDatabaseSeeder
    {
        private readonly BlazorHeroContext _db;
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(UserManager<BlazorHeroUser> userManager, RoleManager<IdentityRole> roleManager, BlazorHeroContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
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
                var existingRole = await _roleManager.FindByNameAsync(Constants.SuperAdminRole);
                if (existingRole != null)
                {
                    return;
                }
                var adminRole = new IdentityRole(Constants.SuperAdminRole);
                await _roleManager.CreateAsync(adminRole);
                var adminUser = new BlazorHeroUser
                {
                    FirstName = "Mukesh",
                    LastName = "Murugan",
                    Email = "mukesh@blazorhero.com",
                    UserName = "mukesh@blazorhero.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await _userManager.CreateAsync(adminUser, Constants.DefaultPassword);
                await _userManager.AddToRoleAsync(adminUser, Constants.SuperAdminRole);
            }).GetAwaiter().GetResult();
        }
    }
}