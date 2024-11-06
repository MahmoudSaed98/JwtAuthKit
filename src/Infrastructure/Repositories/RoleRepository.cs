using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RoleRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Delete(Role entity)
    {
        _dbContext.Set<Role>().Remove(entity);
    }

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
                               .AsNoTracking()
                               .ToListAsync(cancellationToken);
    }

    public async Task<Role?> GetByIdAsync(int key, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
                               .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public async Task<Role?> GetRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Role>()
                               .FirstOrDefaultAsync(x => x.Name == role, cancellationToken);
    }

    public void Insert(Role entity)
    {
        _dbContext.Set<Role>().Add(entity);
    }

    public async Task<bool> IsRoleExists(string role, CancellationToken cancellationToken = default)
    {
        return await
             _dbContext.Set<Role>()
                       .AnyAsync(x => x.Name == role, cancellationToken);
    }

    public void Update(Role entity)
    {
        _dbContext.Set<Role>().Update(entity);
    }
}
