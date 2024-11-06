using Application.Common;
using Application.DTOs.Requests;
using Application.DTOs.Responses;

namespace Application.Interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionResponse>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);

    Task<Result<PermissionResponse>> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Result<string>> GrantPermissionToRoleAsync(GrantPermissionRequest request, CancellationToken cancellationToken = default);

    Task<Result<string>> RevokePermissionFromRoleAsync(int permissionId, int roleId, CancellationToken cancellationToken = default);
}
