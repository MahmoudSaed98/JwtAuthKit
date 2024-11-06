using Domain.Enums;

namespace Application.Interfaces;

public interface IPermissionManager
{
    bool IsAdmin(Permissions permissions);

    bool HasPermission(Permissions userPermissions, Permissions requiredPermissions);

    Permissions GrantPermission(Permissions existingPermissions, Permissions permissionsToGrant);

    Permissions RevokePermission(Permissions existingPermissions, Permissions permissionsToRevoke);
}