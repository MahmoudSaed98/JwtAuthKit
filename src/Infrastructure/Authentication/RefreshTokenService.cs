using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Settings;
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

        string generatedRefreshTokenValue = GenerateRefreshTokenInternal();

        var newRefreshToken = RefreshToken.Create(generatedRefreshTokenValue, userId, _dateTimeProvider.UtcNow, expires);

        _refreshTokenRepository.Insert(newRefreshToken);

        return newRefreshToken;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _refreshTokenRepository.
            GetAsync(refreshToken, cancellationToken);
    }

    public async Task RevokeAllRefreshTokensForUserAsync(int userId, CancellationToken cancellationToken = default)
    {
        await _refreshTokenRepository.
            RevokeAllRefreshTokensForUserAsync(userId, cancellationToken);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        var tokenToRevoke = await _refreshTokenRepository.GetAsync(refreshToken);

        if (tokenToRevoke != null)
        {
            tokenToRevoke.Revoke();
        }

        throw new Exception("token not found.");
    }

    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            return false;
        }

        return await _refreshTokenRepository.
            IsValidRefreshToken(refreshToken, cancellationToken);
    }

    private static string GenerateRefreshTokenInternal()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    }

    public void Delete(RefreshToken refreshToken)
    {
        _refreshTokenRepository.Remove(refreshToken);
    }
}
