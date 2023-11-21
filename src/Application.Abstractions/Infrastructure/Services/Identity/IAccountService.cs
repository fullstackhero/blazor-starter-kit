using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Wrapper;

namespace CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;

public interface IAccountService
{
    Task<Result> UpdateProfileAsync(UpdateProfileRequest model, string userId);

    Task<Result> ChangePasswordAsync(ChangePasswordRequest model, string userId);

    Task<Result<string>> GetProfilePictureAsync(string userId);

    Task<Result<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
}
