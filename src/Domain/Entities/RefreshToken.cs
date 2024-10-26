namespace Domain.Entities;

public sealed class RefreshToken : Entity<int>
{
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime IssuedAt { get; private set; }
    public int UserId { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    private RefreshToken(string token, int userId, DateTime expiresAt)
    {
        ArgumentException.ThrowIfNullOrEmpty(token, nameof(token));
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(expiresAt, DateTime.UtcNow);

        this.Token = token;
        this.ExpiresAt = expiresAt;
        this.IssuedAt = DateTime.UtcNow;
        this.UserId = userId;
    }

    public static RefreshToken Create(string token, int userId, DateTime expiresAt) =>
                                                           new(token, userId, expiresAt);
    public void Revoke()
    {
        if (IsRevoked)
            return;

        IsRevoked = true;
        RevokedAt = DateTime.Now;
    }

    private RefreshToken() { } // called by ef 

}
