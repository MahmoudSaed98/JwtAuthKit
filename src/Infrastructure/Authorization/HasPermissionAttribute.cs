using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute() { }

    public HasPermissionAttribute(string policy) => Policy = policy;

    public HasPermissionAttribute(Permissions permission) => Permissions = permission;

    public Permissions Permissions
    {
        get
        {
            return !string.IsNullOrEmpty(Policy) ?
                PolicyHelper.GetPolicyPermissions(Policy) : Permissions.None;
        }
        set
        {
            Policy = value != Permissions.None ?
                PolicyHelper.GeneratePolicyNameFor(value) : string.Empty;
        }
    }
}
