using Domain.Entities;
using Infrastructure.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

internal sealed class PermissionService : IPermissionService
{
    private readonly ApplicationDbContext _dbContext;

    public PermissionService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HashSet<string>> GetPermissionsAsync(string username)
    {
        var rolesCollections = await _dbContext.Set<User>()
                                    .AsSplitQuery()
                                    .Include(user => user.Roles)
                                    .ThenInclude(role => role.Permissions)
                                    .Where(user => user.Username == username)
                                    .Select(user => user.Roles)
                                    .ToArrayAsync();

        return rolesCollections
              .SelectMany(roles => roles)
              .SelectMany(role => role.Permissions)
              .Select(permission => permission.Name)
              .ToHashSet();
    }
}
