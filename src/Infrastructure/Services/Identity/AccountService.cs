using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Infrastructure.Models.Identity;
using CleanBlazor.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Infrastructure.Services.Identity;

public class AccountService : IAccountService
{
    private readonly IStringLocalizer<AccountService> _localizer;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IUploadService _uploadService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IUploadService uploadService,
        IStringLocalizer<AccountService> localizer)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _uploadService = uploadService;
        _localizer = localizer;
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordRequest model, string userId)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Fail(_localizer["User Not Found."]);
        }

        IdentityResult identityResult = await _userManager.ChangePasswordAsync(
            user,
            model.Password,
            model.NewPassword);
        List<string> errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
        return identityResult.Succeeded ? Result.Ok() : Result.Fail(errors);
    }

    public async Task<Result> UpdateProfileAsync(UpdateProfileRequest model, string userId)
    {
        if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
        {
            ApplicationUser userWithSamePhoneNumber =
                await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return Result.Fail(string.Format(_localizer["Phone number {0} is already used."],
                    model.PhoneNumber));
            }
        }

        ApplicationUser userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
        if (userWithSameEmail != null && userWithSameEmail.Id != userId)
        {
            return Result.Fail(string.Format(_localizer["Email {0} is already used."], model.Email));
        }

        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Fail(_localizer["User Not Found."]);
        }

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (model.PhoneNumber != phoneNumber)
        {
            await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
        }

        IdentityResult identityResult = await _userManager.UpdateAsync(user);
        List<string> errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
        await _signInManager.RefreshSignInAsync(user);
        return identityResult.Succeeded ? Result.Ok() : Result.Fail(errors);
    }

    public async Task<Result<string>> GetProfilePictureAsync(string userId)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Fail<string>(_localizer["User Not Found"]);
        }

        return user.ProfilePictureDataUrl;
    }

    public async Task<Result<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Fail<string>(_localizer["User Not Found"]);
        }

        var filePath = _uploadService.UploadAsync(request);
        user.ProfilePictureDataUrl = filePath;
        IdentityResult identityResult = await _userManager.UpdateAsync(user);
        List<string> errors = identityResult.Errors.Select(e => _localizer[e.Description].ToString()).ToList();
        return identityResult.Succeeded
            ? filePath
            : Result.Fail<string>(errors);
    }
}
