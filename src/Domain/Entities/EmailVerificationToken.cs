namespace Domain.Entities;

public class EmailVerificationToken : Entity<int>
{
    public int UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public User User { get; } = null!;
    private EmailVerificationToken(int userId, string token, DateTime createdAt, DateTime expiresAt) =>
        (UserId, Token, CreatedAt, ExpiresAt) = (userId, token, createdAt, expiresAt);

    public static EmailVerificationToken Create(int userId, string token, DateTime createdAt, DateTime expiresAt) =>
                                         new(userId, token, createdAt, expiresAt);

    private EmailVerificationToken() { }

    public static EmailVerificationToken Default => new();
}