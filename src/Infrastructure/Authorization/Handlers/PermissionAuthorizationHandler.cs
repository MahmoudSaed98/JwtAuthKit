using Infrastructure.Authorization.Requirements;
using Infrastructure.Claims;
using Infrastructure.Services;
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

    protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        string? username = context.User.Claims.
                FirstOrDefault(claim => claim.Type == CustomClaims.Username)?.Value;

        if (username is null)
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        HashSet<string> permissions = await permissionService.GetPermissionsAsync(username);

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}
