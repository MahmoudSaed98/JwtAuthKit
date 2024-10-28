using Domain.Entities;

namespace Application.Interfaces;

public interface IRefreshTokenService
{
    RefreshToken GenerateRefreshToken(int userId);

    void Delete(RefreshToken refreshToken);

    Task<bool> ValidateRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

    Task RevokeAllRefreshTokensForUserAsync(int userId, CancellationToken cancellationToken = default);
}
