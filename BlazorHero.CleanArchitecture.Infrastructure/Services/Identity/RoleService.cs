using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Helpers;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using BlazorHero.CleanArchitecture.Application.Models.Identity;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Identity
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

        public async Task<Result<List<RoleResponse>>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
            return Result<List<RoleResponse>>.Success(rolesResponse);
        }

        public async Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId)
        {
            var model = new PermissionResponse();
            var allPermissions = new List<RoleClaimsResponse>();

            #region GetPermissions

            allPermissions.GetPermissions(typeof(Permissions.Users), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Roles), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Products), roleId);
            allPermissions.GetPermissions(typeof(Permissions.Brands), roleId);
            //You could have your own method to refactor the below line, maybe by using Reflection and fetch directly from a class, else assume that Admin has all the roles assigned and retreive the Admin's roles here via the DB/Identity.RoleClaims table.
            allPermissions.Add(new RoleClaimsResponse { Value = "Permissions.Communication.Chat", Type = ApplicationClaimTypes.Permission });

            #endregion GetPermissions

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                model.RoleId = role.Id;
                model.RoleName = role.Name;
                var claims = await _roleManager.GetClaimsAsync(role);
                var allClaimValues = allPermissions.Select(a => a.Value).ToList();
                var roleClaimValues = claims.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
                foreach (var permission in allPermissions)
                {
                    if (authorizedClaims.Any(a => a == permission.Value))
                    {
                        permission.Selected = true;
                    }
                }
            }
            model.RoleClaims = allPermissions;
            return Result<PermissionResponse>.Success(model);
        }

        public async Task<Result<RoleResponse>> GetByIdAsync(string id)
        {
            var roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
            var rolesResponse = _mapper.Map<RoleResponse>(roles);
            return Result<RoleResponse>.Success(rolesResponse);
        }

        public async Task<Result<string>> SaveAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null) return Result<string>.Fail($"Similar Role already exists.");
                var response = await _roleManager.CreateAsync(new IdentityRole(request.Name));
                return Result<string>.Success("Role Created");
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                if (existingRole.Name == "Administrator" || existingRole.Name == "Basic")
                {
                    return Result<string>.Fail($"Not allowed to modify {existingRole.Name} Role.");
                }
                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                await _roleManager.UpdateAsync(existingRole);
                return Result<string>.Success("Role Updated.");
            }
        }

        public async Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role.Name == "Administrator")
                {
                    return Result<string>.Fail($"Not allowed to modify Permissions for this Role.");
                }
                var claims = await _roleManager.GetClaimsAsync(role);
                foreach (var claim in claims)
                {
                    await _roleManager.RemoveClaimAsync(role, claim);
                }
                var selectedClaims = request.RoleClaims.Where(a => a.Selected).ToList();
                foreach (var claim in selectedClaims)
                {
                    await _roleManager.AddPermissionClaim(role, claim.Value);
                }
                return Result<string>.Success("Permission Updated.");
            }
            catch (Exception ex)
            {
                return Result<string>.Fail(ex.Message);
            }
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _roleManager.Roles.CountAsync();
            return count;
        }
    }
}