using CleanBlazor.Contracts.Identity;
using CleanBlazor.Shared.Wrapper;

namespace CleanBlazor.Application.Abstractions.Infrastructure.Services.Identity;

public interface ITokenService
{
    Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

    Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
}
