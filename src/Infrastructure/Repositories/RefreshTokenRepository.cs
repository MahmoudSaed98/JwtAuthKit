using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RefreshToken?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        return await _dbContext
                              .Set<RefreshToken>()
                              .FirstOrDefaultAsync(t => t.Token == token, cancellationToken);
    }

    public async Task<RefreshToken?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext
                               .Set<RefreshToken>()
                               .FirstOrDefaultAsync(r => r.UserId == userId, cancellationToken);
    }

    public void Insert(RefreshToken refreshToken)
    {
        _dbContext.Set<RefreshToken>().Add(refreshToken);
    }

    public async Task<bool> IsRefreshTokenExpiredAsync(string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _dbContext
                         .Set<RefreshToken>()
                         .AnyAsync(r => DateTime.UtcNow >= r.ExpiresAt, cancellationToken);
    }

    public void Remove(RefreshToken refreshToken)
    {
        _dbContext.Set<RefreshToken>().Remove(refreshToken);
    }
}
