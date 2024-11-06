using Domain.Entities;

namespace Domain.Abstractions;

public interface IPermissionRepository : IRepository<int, Permission>
{
    Task<bool> IsPermissionExistsAsync(string permissionId, CancellationToken cancellationToken = default);

    Task<Permission?> GetPermissionByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<IEnumerable<Permission>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
}