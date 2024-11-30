using Application.Common;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<UserResponse>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);

    Task<Result<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);

    Task<bool> VerifyEmailAsync(string verificationToken, CancellationToken cancellationToken = default);

    Task<Result<string>> ResendVerificationEmailAsync(int userId, CancellationToken cancellationToken = default);

    Task<Result<string>> RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request, CancellationToken cancellationToken = default);

    Task<Result<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);

}