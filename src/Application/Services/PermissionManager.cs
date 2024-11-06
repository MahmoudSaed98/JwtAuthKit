using Application.Interfaces;
using Domain.Enums;

namespace Application.Services;

public class PermissionManager : IPermissionManager
{
    public bool HasPermission(Permissions userPermissions, Permissions requiredPermissions)
    {
        return (userPermissions & requiredPermissions) == requiredPermissions;
    }

    public Permissions GrantPermission(Permissions existingPermissions, Permissions permissionsToGrant)
    {
        return existingPermissions | permissionsToGrant;
    }

    public Permissions RevokePermission(Permissions existingPermissions, Permissions permissionsToRevoke)
    {
        return existingPermissions & ~permissionsToRevoke;
    }

    public bool IsAdmin(Permissions permissions)
    {
        //return HasPermission(permissions,
        //    Permissions.CanAdd | Permissions.CanRead | Permissions.CanUpdate | Permissions.CanDelete);

        return HasPermission(permissions, Permissions.All);
    }
}
