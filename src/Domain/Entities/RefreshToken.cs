namespace Domain.Entities;

public sealed class RefreshToken : Entity<int>
{
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public int UserId { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    private RefreshToken(string token, int userId, DateTime issuedAt, DateTime expiresAt)
    {
        ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(expiresAt, issuedAt);

        this.Token = token;
        this.ExpiresAt = expiresAt;
        this.IssuedAt = issuedAt;
        this.UserId = userId;
    }

    public static RefreshToken Create(string token, int userId, DateTime issuedAt, DateTime expiresAt) =>
                                                           new(token, userId, issuedAt, expiresAt);
    public void Revoke()
    {
        if (IsRevoked)
            return;

        IsRevoked = true;
        RevokedAt = DateTime.Now;
    }

    private RefreshToken() { } // called by ef 

}
