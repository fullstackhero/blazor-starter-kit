using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorHero.CleanArchitecture.UserService.Interfaces.Requests;
using BlazorHero.CleanArchitecture.UserService.Interfaces.Responses;
using BlazorHero.CleanArchitecture.Utils;
using BlazorHero.CleanArchitecture.Utils.Wrapper;

namespace BlazorHero.CleanArchitecture.UserService.Interfaces
{
    public interface IUserService : IService
    {
        Task<Result<List<UserResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<IResult<UserResponse>> GetAsync(string userId);

        Task<IResult> RegisterAsync(RegisterRequest request, string origin);

        Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request);

        Task<IResult<UserRolesResponse>> GetRolesAsync(string id);

        Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request);

        Task<IResult<string>> ConfirmEmailAsync(string userId, string code);

        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
    }
}