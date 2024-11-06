using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal class UserRepository : Repository<int, User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context)
        : base(context) { }

    public override Task<User?> GetByIdAsync(int key, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<User>()
                        .Include(x => x.Roles)
                        .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>()
                               .Include(x => x.Roles)
                              .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<User>()
                              .Include(user => user.Roles)
                              .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public Task<User?> GetByUsernameAsync(string? username, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<User>().Include(user => user.Roles)
                        .FirstOrDefaultAsync(x => x.Username == username, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
    {
        return !await dbContext.Set<User>().AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken = default)
    {
        return !await dbContext.Set<User>().AnyAsync(x => x.Username == username, cancellationToken);
    }
}
