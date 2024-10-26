using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Infrastructure.Authentication;

internal sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    private readonly IUserRepository _userRepository;

    private readonly IDateTimeProvider _dateTimeProvider;

    private readonly JwtSettings _jwtSettings;

    public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository,
        IUserRepository userRepository, IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> options)
    {
        ArgumentNullException.ThrowIfNull(refreshTokenRepository);
        ArgumentNullException.ThrowIfNull(userRepository);
        ArgumentNullException.ThrowIfNull(dateTimeProvider);
        ArgumentNullException.ThrowIfNull(options);

        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = options.Value;
    }

    public RefreshToken GenerateRefreshToken(int userId)
    {
        var expires = _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.RefreshTokenExpirationInMinutes);

        string token = GenerateRefreshTokenInternal();

        var tokenEntity = RefreshToken.Create(token, userId, expires);

        _refreshTokenRepository.Insert(tokenEntity);

        return tokenEntity;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenRepository.GetAsync(refreshToken, cancellationToken);
    }

    public async Task RevokeAllRefreshTokensForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        var selectedUser = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (selectedUser is null)
        {
            throw new Exception($"user with {userId} was not found.");
        }

        foreach (var token in selectedUser.RefreshTokens)
        {
            token.Revoke();
        }
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenToRevoke = await _refreshTokenRepository.GetAsync(refreshToken);

        tokenToRevoke!.Revoke();
    }

    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new Exception("refresh token is null or empty");
        }

        var seletedRefreshToken = await _refreshTokenRepository.GetAsync(refreshToken, cancellationToken);

        if (seletedRefreshToken is null)
            return false;

        return !seletedRefreshToken.IsRevoked &&
                seletedRefreshToken.ExpiresAt > DateTime.UtcNow;
    }

    private static string GenerateRefreshTokenInternal()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }

}
