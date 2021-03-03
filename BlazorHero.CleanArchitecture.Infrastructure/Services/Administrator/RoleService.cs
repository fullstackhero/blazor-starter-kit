using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Administrator;
using BlazorHero.CleanArchitecture.Application.Requests.Roles;
using BlazorHero.CleanArchitecture.Application.Responses.Roles;
using BlazorHero.CleanArchitecture.Application.Wrapper;
using BlazorHero.CleanArchitecture.Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Administrator
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlazorHeroUser> _userManager;
        private IMapper _mapper;
        public RoleService(RoleManager<IdentityRole> roleManager, IMapper mapper, UserManager<BlazorHeroUser> userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Result<string>> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole.Name != "Administrator" && existingRole.Name != "Basic")
            {
                //TODO Check if Any Users already uses this Role
                bool roleIsNotUsed = true;
                var allUsers = await _userManager.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                    {
                        roleIsNotUsed = false;
                    }
                }
                if (roleIsNotUsed)
                {
                    await _roleManager.DeleteAsync(existingRole);
                    return Result<string>.Success($"Role {existingRole.Name} deleted.");
                }
                else
                {
                    return Result<string>.Fail($"Not allowed to delete {existingRole.Name} Role as it is being used.");
                }
            }
            else
            {
                return Result<string>.Fail($"Not allowed to delete {existingRole.Name} Role.");
            }
        }

        public async Task<Result<GetAllRolesResponse>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.Where(a=>a.Name != Constants.AdministratorRole).ToListAsync();
            var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
            var result = new GetAllRolesResponse { Roles = rolesResponse };
            return Result<GetAllRolesResponse>.Success(result);
        }

        public async Task<Result<string>> SaveAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var response = await _roleManager.CreateAsync(new IdentityRole(request.Name));
                return Result<string>.Success(request.Id);
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                await _roleManager.UpdateAsync(existingRole);
                return Result<string>.Success(existingRole.Id);
            }
        }
    }
}
