using System.Globalization;
using System.Text;
using System.Text.Encodings.Web;
using AutoMapper;
using CleanBlazor.Application.Abstractions.Infrastructure.Services;
using CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;
using CleanBlazor.Application.Exceptions;
using CleanBlazor.Application.Extensions;
using CleanBlazor.Contracts.Identity;
using CleanBlazor.Contracts.Mail;
using CleanBlazor.Infrastructure.Models.Identity;
using CleanBlazor.Infrastructure.Specifications;
using CleanBlazor.Shared.Constants.Role;
using CleanBlazor.Shared.Models.Identity;
using CleanBlazor.Shared.Wrapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace CleanBlazor.Infrastructure.Services.Identity;

public class UserService : IUserService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IExcelService _excelService;
    private readonly IStringLocalizer<UserService> _localizer;
    private readonly IMailService _mailService;
    private readonly IMapper _mapper;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(
        UserManager<ApplicationUser> userManager,
        IMapper mapper,
        RoleManager<ApplicationRole> roleManager,
        IMailService mailService,
        IStringLocalizer<UserService> localizer,
        IExcelService excelService,
        ICurrentUserService currentUserService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _roleManager = roleManager;
        _mailService = mailService;
        _localizer = localizer;
        _excelService = excelService;
        _currentUserService = currentUserService;
    }

    public IQueryable<IUser> Users => _userManager.Users.Cast<IUser>();

    public async Task<Result<List<UserResponse>>> GetAllAsync()
    {
        List<ApplicationUser> users = await _userManager.Users.ToListAsync();
        var result = _mapper.Map<List<UserResponse>>(users);
        return result;
    }

    public async Task<Result> RegisterAsync(RegisterRequest request, string origin)
    {
        ApplicationUser userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            return Result.Fail(string.Format(_localizer["Username {0} is already taken."],
                request.UserName));
        }

        var user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName,
            PhoneNumber = request.PhoneNumber,
            IsActive = request.ActivateUser,
            EmailConfirmed = request.AutoConfirmEmail
        };

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            ApplicationUser userWithSamePhoneNumber =
                await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (userWithSamePhoneNumber != null)
            {
                return Result.Fail(string.Format(_localizer["Phone number {0} is already registered."],
                    request.PhoneNumber));
            }
        }

        ApplicationUser userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail != null)
        {
            return Result.Fail(string.Format(_localizer["Email {0} is already registered."], request.Email));
        }

        IdentityResult result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return Result.Fail(result.Errors.Select(a => _localizer[a.Description].ToString()).ToList());
        }

        await _userManager.AddToRoleAsync(user, RoleConstants.BasicRole);
        if (request.AutoConfirmEmail)
        {
            return Result.Ok(user.Id, string.Format(_localizer["User {0} Registered."], user.UserName));
        }

        var verificationUri = await SendVerificationEmail(user, origin);
        var mailRequest = new MailRequest
        {
            From = "mail@codewithmukesh.com",
            To = user.Email,
            Body = string.Format(
                _localizer["Please confirm your account by <a href='{0}'>clicking here</a>."],
                verificationUri),
            Subject = _localizer["Confirm Registration"]
        };
        BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
        return Result.Ok(user.Id,
            string.Format(_localizer["User {0} Registered. Please check your Mailbox to verify!"], user.UserName));
    }

    public async Task<Result<UserResponse>> GetAsync(string userId)
    {
        ApplicationUser user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
        var result = _mapper.Map<UserResponse>(user);
        return result;
    }

    public async Task<Result> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        ApplicationUser user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null)
        {
            return Result.Fail(_localizer["User Not Found."]);
        }

        var isAdmin = await _userManager.IsInRoleAsync(user, RoleConstants.AdministratorRole);
        if (isAdmin)
        {
            return Result.Fail(_localizer["Administrators Profile's Status cannot be toggled"]);
        }

        user.IsActive = request.ActivateUser;
        await _userManager.UpdateAsync(user);

        return Result.Ok();
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }
    public async Task<Result<UserRolesResponse>> GetRolesAsync(string userId)
    {
        var viewModel = new List<UserRoleModel>();
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        List<ApplicationRole> roles = await _roleManager.Roles.ToListAsync();

        foreach (ApplicationRole role in roles)
        {
            var userRolesViewModel = new UserRoleModel { RoleName = role.Name, RoleDescription = role.Description };
            if (await _userManager.IsInRoleAsync(user, role.Name))
            {
                userRolesViewModel.Selected = true;
            }
            else
            {
                userRolesViewModel.Selected = false;
            }

            viewModel.Add(userRolesViewModel);
        }

        var result = new UserRolesResponse { UserRoles = viewModel };
        return result;
    }

    public async Task<Result> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result.Fail(_localizer["User Not Found."]);
        }

        if (user.Email == "mukesh@blazorhero.com")
        {
            return Result.Fail(_localizer["Not Allowed."]);
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        List<UserRoleModel> selectedRoles = request.UserRoles.Where(x => x.Selected).ToList();

        ApplicationUser currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
        if (currentUser == null)
        {
            return Result.Fail(_localizer["User Not Found."]);
        }

        if (!await _userManager.IsInRoleAsync(currentUser, RoleConstants.AdministratorRole))
        {
            var tryToAddAdministratorRole = selectedRoles
                .Exists(x => x.RoleName == RoleConstants.AdministratorRole);
            var userHasAdministratorRole = roles.Any(x => x == RoleConstants.AdministratorRole);
            if ((tryToAddAdministratorRole && !userHasAdministratorRole) ||
                (!tryToAddAdministratorRole && userHasAdministratorRole))
            {
                return Result.Fail(
                    _localizer["Not Allowed to add or delete Administrator Role if you have not this role."]);
            }
        }

        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.AddToRolesAsync(user, selectedRoles.Select(y => y.RoleName));
        return Result.Ok(_localizer["Roles Updated"]);
    }

    public async Task<Result<string>> ConfirmEmailAsync(string userId, string code)
    {
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return Result.Fail<string>(_localizer["An Error has occurred!"]);
        }

        IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return Result.Ok(user.Id,
                string.Format(
                    _localizer[
                        "Account Confirmed for {0}. You can now use the /api/identity/token endpoint to generate JWT."],
                    user.Email));
        }

        throw new ApiException(string.Format(_localizer["An error occurred while confirming {0}"], user.Email));
    }

    public async Task<Result> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            return Result.Fail(_localizer["An Error has occurred!"]);
        }

        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/v1/identity/users/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        var mailRequest = new MailRequest
        {
            Body = string.Format(_localizer["Please reset your password by <a href='{0}'>clicking here</a>."],
                HtmlEncoder.Default.Encode(passwordResetUrl)),
            Subject = _localizer["Reset Password"],
            To = request.Email
        };
        BackgroundJob.Enqueue(() => _mailService.SendAsync(mailRequest));
        return Result.Ok(_localizer["Password Reset Mail has been sent to your authorized Email."]);
    }

    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return Result.Fail(_localizer["An Error has occured!"]);
        }

        IdentityResult result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
        if (!result.Succeeded)
        {
            return Result.Fail(_localizer["An Error has occured!"]);
        }

        return Result.Ok(_localizer["Password Reset Successful!"]);
    }

    public async Task<int> GetCountAsync()
    {
        var count = await _userManager.Users.CountAsync();
        return count;
    }

    public async Task<string> ExportToExcelAsync(string searchString = "")
    {
        var userSpec = new UserFilterSpecification(searchString);
        List<ApplicationUser> users = await _userManager.Users
            .Specify(userSpec)
            .OrderByDescending(a => a.CreatedOn)
            .ToListAsync();
        var result = await _excelService.ExportAsync(users,
            sheetName: _localizer["Users"],
            mappers: new Dictionary<string, Func<ApplicationUser, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["FirstName"], item => item.FirstName },
                { _localizer["LastName"], item => item.LastName },
                { _localizer["UserName"], item => item.UserName },
                { _localizer["Email"], item => item.Email },
                { _localizer["EmailConfirmed"], item => item.EmailConfirmed },
                { _localizer["PhoneNumber"], item => item.PhoneNumber },
                { _localizer["PhoneNumberConfirmed"], item => item.PhoneNumberConfirmed },
                { _localizer["IsActive"], item => item.IsActive },
                {
                    _localizer["CreatedOn (Local)"],
                    item => item.CreatedOn.ToLocalTime()
                        .ToString("G", CultureInfo.CurrentCulture)
                },
                { _localizer["CreatedOn (UTC)"], item => item.CreatedOn.ToString("G", CultureInfo.CurrentCulture) },
                { _localizer["ProfilePictureDataUrl"], item => item.ProfilePictureDataUrl }
            });

        return result;
    }

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/v1/identity/users/confirm-email";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        return verificationUri;
    }
}
