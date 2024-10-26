namespace Infrastructure.Authentication;

public sealed class JwtSettings
{
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public int AccessTokenExpirationInMinutes { get; init; }
    public int RefreshTokenExpirationInMinutes { get; init; }
}