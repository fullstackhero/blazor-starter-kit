using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.AccountService.Interfaces;
using BlazorHero.CleanArchitecture.AccountService.Interfaces.Requests;
using BlazorHero.CleanArchitecture.Domain.Entities.Identity;
using BlazorHero.CleanArchitecture.UploadService.Interfaces;
using BlazorHero.CleanArchitecture.Utils.Wrapper;
using Microsoft.Extensions.Localization;

namespace BlazorHero.CleanArchitecture.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<BlazorHeroUser> _userManager;
        private readonly SignInManager<BlazorHeroUser> _signInManager;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AccountService> _localizer;

        public AccountService(
            UserManager<BlazorHeroUser> userManager,
            SignInManager<BlazorHeroUser> signInManager,
            IUploadService uploadService,
            IStringLocalizer<AccountService> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result.FailAsync(_localizer["User Not Found."]);
            }

            var identityResult = await this._userManager.ChangePasswordAsync(
                user,
                model.Password,
                model.NewPassword);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
        }

        public async Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return await Result.FailAsync(_localizer["User Not Found."]);
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
            }
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            await _signInManager.RefreshSignInAsync(user);
            return identityResult.Succeeded ? await Result.SuccessAsync() : await Result.FailAsync(errors);
        }

        public async Task<IResult<string>> GetProfilePictureAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return await Result<string>.FailAsync(_localizer["User Not Found"]);
            return await Result<string>.SuccessAsync(data: user.ProfilePictureDataUrl);
        }

        public async Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return await Result<string>.FailAsync(message: _localizer["User Not Found"]);
            var filePath = _uploadService.UploadAsync(request);
            user.ProfilePictureDataUrl = filePath;
            var identityResult = await _userManager.UpdateAsync(user);
            var errors = identityResult.Errors.Select(e => e.Description).ToList();
            return identityResult.Succeeded ? await Result<string>.SuccessAsync(data: filePath) : await Result<string>.FailAsync(errors);
        }
    }
}