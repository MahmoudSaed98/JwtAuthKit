using Application.Interfaces;
using Domain.Enums;
using Infrastructure.Authorization.Requirements;
using Infrastructure.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authorization.Handlers;


internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {

        var combinedPermissionsClaims = context.User.Claims.FirstOrDefault(x => x.Type == CustomClaims.Permissions)?.Value;

        if (!long.TryParse(combinedPermissionsClaims, out long parsedPermissonsValue))
        {
            return Task.CompletedTask;
        }

        var userPermissions = (Permissions)parsedPermissonsValue;

        using var scope = _serviceScopeFactory.CreateScope();

        var permissionManager = scope.ServiceProvider.GetRequiredService<IPermissionManager>();

        if (permissionManager.HasPermission(userPermissions, requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
