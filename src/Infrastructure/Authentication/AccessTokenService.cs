using Application.Interfaces;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Claims;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication;

internal sealed class AccessTokenService : IAccessTokenService
{
    private readonly JwtSettings _settings;

    private readonly TokenValidationParameters _tokenValidationParameters;

    private readonly IDateTimeProvider _dateTimeProvider;

    public AccessTokenService(IOptions<JwtSettings> jwtSettings, IOptions<JwtBearerOptions> jwtBearerOptions, IRefreshTokenRepository refreshTokenRepository, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
    {

        ArgumentNullException.ThrowIfNull(jwtSettings, nameof(jwtSettings));
        ArgumentNullException.ThrowIfNull(jwtBearerOptions, nameof(jwtBearerOptions));
        ArgumentNullException.ThrowIfNull(dateTimeProvider, nameof(dateTimeProvider));
        ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));

        _settings = jwtSettings.Value;
        _tokenValidationParameters = jwtBearerOptions.Value.TokenValidationParameters;
        _dateTimeProvider = dateTimeProvider;
    }
    public string GenerateAccessToken(User user)
    {
        string key = _settings.SecretKey;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var issuedAt = DateTime.UtcNow;
        var issuedAtUnix = ((DateTimeOffset)issuedAt).ToUnixTimeSeconds();
        var expiresAt = issuedAt.AddMinutes(_settings.AccessTokenExpirationInMinutes);

        var claims = new[]
        {
          new Claim(CustomPublicClaims.Email,user.Email),

          new Claim(CustomPublicClaims.Username,user.Username),

          new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),

          new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),

          new Claim(JwtRegisteredClaimNames.Iat,issuedAtUnix.ToString())
        };

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Audience = _settings.Audience,
            Issuer = _settings.Issuer,
            Expires = expiresAt,
            NotBefore = issuedAt,
            SigningCredentials = credentials,
        };

        return new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler().CreateToken(descriptor);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredAccessToken(string accessToken)
    {
        var tokenValidationParameters = _tokenValidationParameters;

        tokenValidationParameters.ValidateLifetime = false;

        return VlidateAccessToken(accessToken, tokenValidationParameters);
    }

    private static bool IsTokenHasValidSecurityAlgorithm(SecurityToken token)
    {
        return token is JwtSecurityToken jwtToken
               && jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
               StringComparison.OrdinalIgnoreCase);
    }


    public string? GetEmailClaim(ClaimsPrincipal principal)
    {
        return GetClaim(principal, CustomPublicClaims.Email);
    }

    public string? GetUsernameCliam(ClaimsPrincipal principal)
    {
        return GetClaim(principal, CustomPublicClaims.Username);
    }

    private ClaimsPrincipal VlidateAccessToken(string accessToken, TokenValidationParameters tokenValidationParameters)
    {
        var jwtHandler = new JwtSecurityTokenHandler();

        var claimsPrincipal = jwtHandler.ValidateToken(accessToken, tokenValidationParameters,
              out SecurityToken validatedToken);

        if (!IsTokenHasValidSecurityAlgorithm(validatedToken))
        {
            throw new InvalidAccessTokenException();
        }

        return claimsPrincipal;
    }

    public string? GetClaim(ClaimsPrincipal principal, string claimType)
    {
        return principal.Claims
                        .FirstOrDefault(c => c.Type == claimType)?.Value.Trim();
    }
}
