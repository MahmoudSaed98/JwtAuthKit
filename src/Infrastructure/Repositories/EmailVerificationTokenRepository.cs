using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class EmailVerificationTokenRepository : Repository<int,
    EmailVerificationToken>, IEmailVerificationTokenRepository
{
    public EmailVerificationTokenRepository(ApplicationDbContext context) :
        base(context)
    {
    }

    public Task<EmailVerificationToken?> GetAsync(string token, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<EmailVerificationToken>().Include(x => x.User)
                       .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public override Task<EmailVerificationToken?> GetByIdAsync(int key, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<EmailVerificationToken>().Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public Task<EmailVerificationToken?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<EmailVerificationToken>().Include(x => x.User)
                        .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
    }
}
