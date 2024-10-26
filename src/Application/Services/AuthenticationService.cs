using Application.DTOs.Requests;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;

namespace Application.Services;

internal class AuthenticationService(IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IAccessTokenService accessTokenService,
    IRefreshTokenService refreshTokenService,
    IUnitOfWork unitOfWork) : IAuthenticationService

{
    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid Credentials.");
        }

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid Credentials.");
        }

        var accessToken = accessTokenService.GenerateAccessToken(user);

        var refreshToken = refreshTokenService.GenerateRefreshToken(user.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new LoginResponse(accessToken!, refreshToken.Token);
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {

        if (!await refreshTokenService.ValidateRefreshTokenAsync(request.RefreshToken))
        {
            throw new Exception("Invalid or expired refresh token");
        }

        var claimsPrincipal = accessTokenService.GetPrincipalFromExpiredAccessToken(request.AccessToken);

        if (claimsPrincipal is null)
        {
            throw new Exception("Invalid access token.");
        }

        string? username = accessTokenService.GetUsernameCliam(claimsPrincipal);

        var user = await userRepository.GetByUsernameAsync(username!.Trim(), cancellationToken);

        if (user is null)
        {
            throw new Exception("user no longer exist.");
        }

        await refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);

        string newAccessToken = accessTokenService.GenerateAccessToken(user)!;

        var refreshTokenEntity = refreshTokenService.GenerateRefreshToken(user!.Id);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse(newAccessToken!, refreshTokenEntity.Token);
    }

    public async Task<UserResponse> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {

        if (!await userRepository.IsEmailUniqueAsync(request.Email, cancellationToken))
        {
            throw new Exception("email already in use.");
        }

        if (!await userRepository.IsUsernameUniqueAsync(request.Username, cancellationToken))
        {
            throw new Exception("username already in use.");
        }

        string passwordHash = passwordHasher.Hash(request.Password);

        var newUser = User.Create(request.Username, passwordHash, request.Email);

        userRepository.Insert(newUser);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponse(newUser.Id, newUser.Username, newUser.Email);
    }
}
