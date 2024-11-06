using Domain.Entities;
using System.Security.Claims;

namespace Application.Interfaces;

public interface IAccessTokenService
{
    string GenerateAccessToken(User user);

    ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken);

    string? GetEmailClaim(ClaimsPrincipal principal);

    string? GetUsernameCliam(ClaimsPrincipal principal);

    string? GetClaim(ClaimsPrincipal principal, string claimType);
}