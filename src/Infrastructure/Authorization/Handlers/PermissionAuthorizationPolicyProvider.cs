using Infrastructure.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authorization.Handlers;

internal sealed class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
{
    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        : base(options)
    { }
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var policy = await base.GetPolicyAsync(policyName);

        if (policy is null && PolicyHelper.IsValidPolicyName(policyName))
        {

            var requiredPermission = PolicyHelper.GetPolicyPermissions(policyName);

            policy = new AuthorizationPolicyBuilder()
                         .AddRequirements(new PermissionRequirement(requiredPermission))
                         .Build();
        }

        return policy;
    }
}