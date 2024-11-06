using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization.Requirements;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permissions Permission { get; }

    public PermissionRequirement(Permissions permission)
    {
        Permission = permission;
    }
}