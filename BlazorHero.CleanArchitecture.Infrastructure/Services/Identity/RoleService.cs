using AutoMapper;
using BlazorHero.CleanArchitecture.Application.Helpers;
using BlazorHero.CleanArchitecture.Application.Interfaces.Services.Identity;
using BlazorHero.CleanArchitecture.Application.Models.Identity;
using BlazorHero.CleanArchitecture.Application.Requests.Identity;
using BlazorHero.CleanArchitecture.Application.Responses.Identity;
using BlazorHero.CleanArchitecture.Shared.Constants.Permission;
using BlazorHero.CleanArchitecture.Shared.Constants.Role;
using BlazorHero.CleanArchitecture.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.Infrastructure.Services.Identity
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly IStringLocalizer<RoleService> _localizer;
        private readonly IMapper _mapper;

        public RoleService(
            RoleManager<IdentityRole> roleManager,
            IMapper mapper,
            UserManager<BlazorHeroUser> userManager,
            IStringLocalizer<RoleService> localizer)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<Result<string>> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole.Name != RoleConstants.AdministratorRole && existingRole.Name != RoleConstants.BasicRole)
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
                    return await Result<string>.SuccessAsync($"{_localizer["Role"]} {existingRole.Name} {_localizer["deleted."]}");
                }
                else
                {
                    return await Result<string>.FailAsync($"{_localizer["Not allowed to delete"]} {existingRole.Name} {_localizer["Role as it is being used."]}");
                }
            }
            else
            {
                return await Result<string>.FailAsync($"{_localizer["Not allowed to delete"]} {existingRole.Name} {_localizer["Role"]}.");
            }
        }

        public async Task<Result<List<RoleResponse>>> GetAllAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
            return await Result<List<RoleResponse>>.SuccessAsync(rolesResponse);
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
            allPermissions.GetPermissions(typeof(Permissions.Preferences), roleId);
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
            return await Result<PermissionResponse>.SuccessAsync(model);
        }

        public async Task<Result<RoleResponse>> GetByIdAsync(string id)
        {
            var roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
            var rolesResponse = _mapper.Map<RoleResponse>(roles);
            return await Result<RoleResponse>.SuccessAsync(rolesResponse);
        }

        public async Task<Result<string>> SaveAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null) return await Result<string>.FailAsync(_localizer["Similar Role already exists."]);
                var response = await _roleManager.CreateAsync(new IdentityRole(request.Name));
                if (response.Succeeded)
                {
                    return await Result<string>.SuccessAsync(_localizer["Role Created"]);
                }
                else
                {
                    return await Result<string>.FailAsync(response.Errors.Select(e => _localizer[e.Description].Value).ToList());
                }
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                if (existingRole.Name == RoleConstants.AdministratorRole || existingRole.Name == RoleConstants.BasicRole)
                {
                    return await Result<string>.FailAsync($"{_localizer["Not allowed to modify"]} {existingRole.Name} {_localizer["Role"]}.");
                }
                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                await _roleManager.UpdateAsync(existingRole);
                return await Result<string>.SuccessAsync(_localizer["Role Updated."]);
            }
        }

        public async Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role.Name == RoleConstants.AdministratorRole)
                {
                    return await Result<string>.FailAsync(_localizer["Not allowed to modify Permissions for this Role."]);
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
                return await Result<string>.SuccessAsync(_localizer["Permission Updated."]);
            }
            catch (Exception ex)
            {
                return await Result<string>.FailAsync(ex.Message);
            }
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _roleManager.Roles.CountAsync();
            return count;
        }
    }
}