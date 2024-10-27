using Domain.Entities;

namespace Domain.Abstractions;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken = default);

    void Insert(RefreshToken refreshToken);

    void Remove(RefreshToken refreshToken);

    Task<bool> IsValidRefreshToken(string refreshToken, CancellationToken cancellationToken = default);

    Task<RefreshToken?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<bool> RevokeAllRefreshTokensForUserAsync(int userId, CancellationToken cancellationToken = default);
}
