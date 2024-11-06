using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

internal sealed class PermissionRepository : IPermissionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PermissionRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Delete(Permission entity)
    {
        _dbContext.Remove(entity);
    }

    public async Task<Permission?> GetByIdAsync(int key, CancellationToken cancellationToken = default)
    {
        return await
               _dbContext.Set<Permission>()
                         .FirstOrDefaultAsync(x => x.Id == key, cancellationToken);
    }

    public async Task<Permission?> GetPermissionByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await
              _dbContext.Set<Permission>()
                        .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
    }

    public void Insert(Permission entity)
    {
        _dbContext.Set<Permission>().Add(entity);
    }

    public async Task<bool> IsPermissionExistsAsync(string permission, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Permission>().AnyAsync(x => x.Name == permission, cancellationToken);
    }

    public void Update(Permission entity)
    {
        _dbContext.Set<Permission>().Update(entity);
    }

    public async Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Permission>().ToListAsync(cancellationToken);
    }
}
