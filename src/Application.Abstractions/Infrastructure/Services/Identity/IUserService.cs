using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Wrapper;

namespace CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;

public interface IUserService
{
    IQueryable<IUser> Users { get; }
    Task<Result<List<UserResponse>>> GetAllAsync();

    Task<int> GetCountAsync();

    Task<Result<UserResponse>> GetAsync(string userId);

    Task<Result> RegisterAsync(RegisterRequest request, string origin);

    Task<Result> ToggleUserStatusAsync(ToggleUserStatusRequest request);

    Task<bool> IsInRoleAsync(string userId, string role);
    Task<Result<UserRolesResponse>> GetRolesAsync(string userId);

    Task<Result> UpdateRolesAsync(UpdateUserRolesRequest request);

    Task<Result<string>> ConfirmEmailAsync(string userId, string code);

    Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);

    Task<string> ExportToExcelAsync(string searchString = "");
}
