using System.Security.Claims;
using AutoMapper;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Infrastructure.Helpers;
using CleanBlazor.Infrastructure.Models.Identity;
using CleanBlazor.Shared.Constants.Permission;
using CleanBlazor.Shared.Constants.Role;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Infrastructure.Services.Identity;

public class RoleService : IRoleService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IStringLocalizer<RoleService> _localizer;
    private readonly IMapper _mapper;
    private readonly IRoleClaimService _roleClaimService;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public RoleService(
        RoleManager<ApplicationRole> roleManager,
        IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IRoleClaimService roleClaimService,
        IStringLocalizer<RoleService> localizer,
        ICurrentUserService currentUserService)
    {
        _roleManager = roleManager;
        _mapper = mapper;
        _userManager = userManager;
        _roleClaimService = roleClaimService;
        _localizer = localizer;
        _currentUserService = currentUserService;
    }

    public async Task<Result<string>> DeleteAsync(string id)
    {
        ApplicationRole existingRole = await _roleManager.FindByIdAsync(id);
        if (existingRole == null)
        {
            return Result.Fail<string>(_localizer["Role Not Found."]);
        }

        if (existingRole.Name is RoleConstants.AdministratorRole or RoleConstants.BasicRole)
        {
            return Result.Fail<string>(string.Format(_localizer["Not allowed to delete {0} Role."],
                existingRole.Name));
        }

        var roleIsUsed = false;
        List<ApplicationUser> allUsers = await _userManager.Users.ToListAsync();
        foreach (ApplicationUser user in allUsers)
        {
            if (await _userManager.IsInRoleAsync(user, existingRole.Name))
            {
                roleIsUsed = true;
                break;
            }
        }

        if (roleIsUsed)
        {
            return Result.Fail<string>(
                string.Format(_localizer["Not allowed to delete {0} Role as it is being used."], existingRole.Name));
        }

        await _roleManager.DeleteAsync(existingRole);
        return Result.Ok<string>(string.Format(_localizer["Role {0} Deleted."],
            existingRole.Name));
    }

    public async Task<Result<List<RoleResponse>>> GetAllAsync()
    {
        List<ApplicationRole> roles = await _roleManager.Roles.ToListAsync();
        var rolesResponse = _mapper.Map<List<RoleResponse>>(roles);
        return rolesResponse;
    }

    public async Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId)
    {
        var model = new PermissionResponse();
        List<RoleClaimResponse> allPermissions = GetAllPermissions();
        ApplicationRole role = await _roleManager.FindByIdAsync(roleId);
        if (role != null)
        {
            model.RoleId = role.Id;
            model.RoleName = role.Name;
            Result<List<RoleClaimResponse>> roleClaimsResult = await _roleClaimService.GetAllByRoleIdAsync(role.Id);
            if (roleClaimsResult.IsSuccess)
            {
                model.RoleClaims = ModifyPermissions(allPermissions, roleClaimsResult.Data);
            }
            else
            {
                model.RoleClaims = new List<RoleClaimResponse>();
                return Result.Fail<PermissionResponse>(roleClaimsResult.ErrorMessages);
            }
        }

        model.RoleClaims = allPermissions;
        return model;
    }

    public async Task<Result<RoleResponse>> GetByIdAsync(string id)
    {
        ApplicationRole roles = await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
        var rolesResponse = _mapper.Map<RoleResponse>(roles);
        return rolesResponse;
    }

    public async Task<Result<string>> SaveAsync(RoleRequest request)
    {
        if (string.IsNullOrEmpty(request.Id))
        {
            ApplicationRole existingRole = await _roleManager.FindByNameAsync(request.Name);
            if (existingRole != null)
            {
                return Result.Fail<string>(_localizer["Similar Role already exists."]);
            }

            IdentityResult response =
                await _roleManager.CreateAsync(new ApplicationRole(request.Name, request.Description));
            if (!response.Succeeded)
            {
                return Result.Fail<string>(response.Errors.Select(e => _localizer[e.Description].ToString())
                    .ToList());
            }

            return Result.Ok<string>(string.Format(_localizer["Role {0} Created."], request.Name));
        }
        else
        {
            ApplicationRole existingRole = await _roleManager.FindByIdAsync(request.Id);
            if (existingRole == null)
            {
                return Result.Fail<string>(_localizer["Role Not Found."]);
            }

            existingRole.Name = request.Name;
            existingRole.NormalizedName = request.Name.ToUpper();
            existingRole.Description = request.Description;
            await _roleManager.UpdateAsync(existingRole);
            return Result.Ok<string>(string.Format(_localizer["Role {0} Updated."], existingRole.Name));
        }
    }

    public async Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request)
    {
        // TODO: Transaction
        try
        {
            var errors = new List<string>();
            ApplicationRole role = await _roleManager.FindByIdAsync(request.RoleId);
            if (!IsAllowedRoleModification(role, request, out var disallowedMessage))
            {
                return Result.Fail<string>(disallowedMessage);
            }

            await RemoveExistingClaims(role);
            await AddSelectedClaims(request, role, errors);
            await UpdateRoleClaims(request, role, errors);

            return errors.Any()
                ? Result.Fail<string>(errors)
                : Result.Ok<string>(_localizer["Permissions Updated."]);
        }
        catch (Exception ex)
        {
            return Result.Fail<string>(ex.Message);
        }
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _roleManager.Roles.CountAsync();
        return count;
    }

    private static List<RoleClaimResponse> ModifyPermissions(
        List<RoleClaimResponse> allPermissions,
        IReadOnlyCollection<RoleClaimResponse> roleClaims)
    {
        List<string> allClaimValues = allPermissions.Select(a => a.Value).ToList();
        List<string> roleClaimValues = roleClaims.Select(a => a.Value).ToList();
        List<string> authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
        foreach (RoleClaimResponse permission in allPermissions.Where(p =>
                     authorizedClaims.Exists(claim => claim == p.Value)))
        {
            permission.Selected = true;
            RoleClaimResponse roleClaim = roleClaims.SingleOrDefault(a => a.Value == permission.Value);
            if (roleClaim?.Description != null)
            {
                permission.Description = roleClaim.Description;
            }

            if (roleClaim?.Group != null)
            {
                permission.Group = roleClaim.Group;
            }
        }

        return allPermissions;
    }

    private bool IsAllowedRoleModification(ApplicationRole role, PermissionRequest request, out string disallowedMessage)
    {
        disallowedMessage = string.Empty;
        if (role.Name == RoleConstants.AdministratorRole)
        {
            ApplicationUser currentUser = _userManager.Users.SingleAsync(x => x.Id == _currentUserService.UserId).Result;
            if (_userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole).Result)
            {
                disallowedMessage = _localizer["Not allowed to modify Permissions for this Role."].ToString();
                return false;
            }
        }

        List<RoleClaimRequest> selectedClaims = request.RoleClaims.Where(a => a.Selected).ToList();
        if (role.Name != RoleConstants.AdministratorRole ||
            !selectedClaims.TrueForAll(x => x.Value != Permissions.Roles.View) ||
            !selectedClaims.TrueForAll(x => x.Value != Permissions.RoleClaims.View) ||
            !selectedClaims.TrueForAll(x => x.Value != Permissions.RoleClaims.Edit))
        {
            return true;
        }

        disallowedMessage = string.Format(
            _localizer["Not allowed to deselect {0} or {1} or {2} for this Role."],
            Permissions.Roles.View,
            Permissions.RoleClaims.View,
            Permissions.RoleClaims.Edit);
        return false;
    }

    private async Task RemoveExistingClaims(ApplicationRole role)
    {
        IList<Claim> claims = await _roleManager.GetClaimsAsync(role);
        foreach (Claim claim in claims)
        {
            await _roleManager.RemoveClaimAsync(role, claim);
        }
    }

    private async Task AddSelectedClaims(PermissionRequest request, ApplicationRole role, List<string> errors)
    {
        List<RoleClaimRequest> selectedClaims = request.RoleClaims.Where(a => a.Selected).ToList();
        foreach (RoleClaimRequest claim in selectedClaims)
        {
            IdentityResult addResult = await _roleManager.AddPermissionClaim(role, claim.Value);
            if (!addResult.Succeeded)
            {
                errors.AddRange(addResult.Errors.Select(e => _localizer[e.Description].ToString()));
            }
        }
    }

    private async Task UpdateRoleClaims(PermissionRequest request, ApplicationRole role, List<string> errors) =>
        await _roleClaimService.GetAllByRoleIdAsync(role.Id)
            .Match(async (_, roleClaims) =>
                {
                    List<RoleClaimRequest> selectedClaims = request.RoleClaims.Where(a => a.Selected).ToList();
                    foreach (RoleClaimRequest claim in selectedClaims)
                    {
                        RoleClaimResponse addedClaim =
                            roleClaims.SingleOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);
                        if (addedClaim != null)
                        {
                            claim.Id = addedClaim.Id;
                            claim.RoleId = addedClaim.RoleId;
                            Result<string> saveResult = await _roleClaimService.SaveAsync(claim);
                            if (saveResult.IsFailure)
                            {
                                errors.AddRange(saveResult.ErrorMessages);
                            }
                        }
                    }
                },
                errorMessages => errors.AddRange(errorMessages));

    private static List<RoleClaimResponse> GetAllPermissions()
    {
        var allPermissions = new List<RoleClaimResponse>();

        #region GetPermissions

        allPermissions.GetAllPermissions();

        #endregion GetPermissions

        return allPermissions;
    }
}
