using AutoMapper;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Infrastructure.Data;
using CleanBlazor.Infrastructure.Models.Identity;
using CleanBlazor.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Infrastructure.Services.Identity;

public class RoleClaimService : IRoleClaimService
{
    private readonly ApplicationDbContext _db;
    private readonly IStringLocalizer<RoleClaimService> _localizer;
    private readonly IMapper _mapper;

    public RoleClaimService(
        IStringLocalizer<RoleClaimService> localizer,
        IMapper mapper,
        ApplicationDbContext db)
    {
        _localizer = localizer;
        _mapper = mapper;
        _db = db;
    }

    public async Task<Result<List<RoleClaimResponse>>> GetAllAsync()
    {
        List<ApplicationRoleClaim> roleClaims = await _db.RoleClaims.ToListAsync();
        var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
        return roleClaimsResponse;
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _db.RoleClaims.CountAsync();
        return count;
    }

    public async Task<Result<RoleClaimResponse>> GetByIdAsync(int id)
    {
        ApplicationRoleClaim roleClaim = await _db.RoleClaims
            .SingleOrDefaultAsync(x => x.Id == id);
        var roleClaimResponse = _mapper.Map<RoleClaimResponse>(roleClaim);
        return roleClaimResponse;
    }

    public async Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId)
    {
        List<ApplicationRoleClaim> roleClaims = await _db.RoleClaims
            .Include(x => x.Role)
            .Where(x => x.RoleId == roleId)
            .ToListAsync();
        var roleClaimsResponse = _mapper.Map<List<RoleClaimResponse>>(roleClaims);
        return roleClaimsResponse;
    }

    public async Task<Result<string>> SaveAsync(RoleClaimRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RoleId))
        {
            return Result.Fail<string>(_localizer["Role is required."]);
        }

        if (request.Id == 0)
        {
            ApplicationRoleClaim existingRoleClaim =
                await _db.RoleClaims
                    .SingleOrDefaultAsync(x =>
                        x.RoleId == request.RoleId && x.ClaimType == request.Type && x.ClaimValue == request.Value);
            if (existingRoleClaim != null)
            {
                return Result.Fail<string>(_localizer["Similar Role Claim already exists."]);
            }

            var roleClaim = _mapper.Map<ApplicationRoleClaim>(request);
            await _db.RoleClaims.AddAsync(roleClaim);
            await _db.SaveChangesAsync();
            return Result.Ok<string>(string.Format(_localizer["Role Claim {0} created."], request.Value));
        }
        else
        {
            ApplicationRoleClaim existingRoleClaim =
                await _db.RoleClaims
                    .Include(x => x.Role)
                    .SingleOrDefaultAsync(x => x.Id == request.Id);
            if (existingRoleClaim == null)
            {
                return Result.Fail<string>(_localizer["Role Claim does not exist."]);
            }

            existingRoleClaim.ClaimType = request.Type;
            existingRoleClaim.ClaimValue = request.Value;
            existingRoleClaim.Group = request.Group;
            existingRoleClaim.Description = request.Description;
            existingRoleClaim.RoleId = request.RoleId;
            _db.RoleClaims.Update(existingRoleClaim);
            await _db.SaveChangesAsync();
            return Result.Ok<string>(string.Format(_localizer["Role Claim {0} for Role {1} updated."],
                request.Value,
                existingRoleClaim.Role.Name));
        }
    }

    public async Task<Result<string>> DeleteAsync(int id)
    {
        ApplicationRoleClaim existingRoleClaim = await _db.RoleClaims
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (existingRoleClaim == null)
        {
            return Result.Fail<string>(_localizer["Role Claim does not exist."]);
        }

        _db.RoleClaims.Remove(existingRoleClaim);
        await _db.SaveChangesAsync();
        return Result.Ok<string>(string.Format(_localizer["Role Claim {0} for {1} Role deleted."],
            existingRoleClaim.ClaimValue,
            existingRoleClaim.Role.Name));
    }
}
