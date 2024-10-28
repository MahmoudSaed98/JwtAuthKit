using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(Permission permission)
        : base(policy: permission.ToString())
    {

    }
}
