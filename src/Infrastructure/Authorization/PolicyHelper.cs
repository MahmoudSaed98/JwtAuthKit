using Domain.Enums;

namespace Infrastructure.Authorization;

internal static class PolicyHelper
{
    public const string Policy_Prefix = "permissions";

    public static bool IsValidPolicyName(string policyName)
    {
        return policyName is not null && policyName.
            StartsWith(Policy_Prefix, StringComparison.OrdinalIgnoreCase);
    }
    public static string GeneratePolicyNameFor(Permissions permissions)
    {
        return $"{Policy_Prefix}{(int)permissions}";
    }

    public static Permissions GetPolicyPermissions(string policyName)
    {
        var permissionsValue = int.Parse(policyName[Policy_Prefix.Length..]);

        return (Permissions)permissionsValue;
    }
}
