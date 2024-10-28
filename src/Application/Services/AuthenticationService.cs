using Application.Common;
using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Extensions;

namespace Application.Services;

internal class AuthenticationService(IUserRepository userRepository, IPasswordHasher passwordHasher,
    IAccessTokenService accessTokenService,
    IRefreshTokenService refreshTokenService,
    IUnitOfWork unitOfWork) : IAuthenticationService

{
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {

        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            return ApiResponse<LoginResponse>.Failure(ErrorMessages.InvalidCredentials);
        }

        bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!verified)
        {
            return ApiResponse<LoginResponse>.Failure(ErrorMessages.InvalidCredentials);
        }

        var accessToken = accessTokenService.GenerateAccessToken(user);

        var refreshToken = refreshTokenService.GenerateRefreshToken(user.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<LoginResponse>.Sucess(new LoginResponse(accessToken!, refreshToken.Token));

    }

    public async Task<ApiResponse<RefreshTokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {

        var storedRefreshToken = await refreshTokenService.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (storedRefreshToken is null)
        {
            return ApiResponse<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenInvalid);
        }

        if (storedRefreshToken.IsRevokedOrExpired())
        {
            return ApiResponse<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenExpired);
        }

        var claimsPrincipal = accessTokenService.GetPrincipalFromExpiredAccessToken(request.AccessToken);

        if (claimsPrincipal is null)
        {
            return ApiResponse<RefreshTokenResponse>.Failure(ErrorMessages.AccessTokenMalformed);
        }

        string? username = accessTokenService.GetUsernameCliam(claimsPrincipal);

        var user = await userRepository.GetByUsernameAsync(username!, cancellationToken);

        if (user!.Id != storedRefreshToken.UserId)
        {
            return ApiResponse<RefreshTokenResponse>.Failure(ErrorMessages.RefreshTokenUserMismatch);
        }

        refreshTokenService.Delete(storedRefreshToken);

        string newAccessToken = accessTokenService.GenerateAccessToken(user)!;

        var newRefreshToken = refreshTokenService.GenerateRefreshToken(user!.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<RefreshTokenResponse>.Sucess(new RefreshTokenResponse(newAccessToken!, newRefreshToken.Token));
    }

    public async Task<ApiResponse<UserResponse>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {

        if (!await userRepository.IsEmailUniqueAsync(request.Email, cancellationToken))
        {
            return ApiResponse<UserResponse>.Failure(ErrorMessages.EmailAlreadyExists);
        }

        if (!await userRepository.IsUsernameUniqueAsync(request.Username, cancellationToken))
        {
            return ApiResponse<UserResponse>.Failure(ErrorMessages.UsernameAlreadyExists);
        }

        string passwordHash = passwordHasher.Hash(request.Password);

        var newUser = User.Create(request.Username, passwordHash, request.Email);

        userRepository.Insert(newUser);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<UserResponse>.Sucess(new UserResponse(newUser.Id, newUser.Username, newUser.Email));
    }

    public async Task<ApiResponse<string>> RevokeRefreshTokenAsync(RevokeRefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        var tokenToRevoke = await refreshTokenService.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (tokenToRevoke is null)
        {
            return ApiResponse<string>.Failure(ErrorMessages.RefreshTokenNotFound);
        }

        tokenToRevoke.Revoke();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiResponse<string>.Sucess("Refresh token has been revoked successfully.");
    }
}
