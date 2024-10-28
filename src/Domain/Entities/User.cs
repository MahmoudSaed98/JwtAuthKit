namespace Domain.Entities;

public class User : Entity<int>
{
    private List<RefreshToken> _refreshTokens = new List<RefreshToken>();
    private List<Role> _roles = new List<Role>();

    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public IReadOnlyList<RefreshToken> RefreshTokens => _refreshTokens;
    public IReadOnlyList<Role> Roles => _roles;
    private User(string username, string passwordHash, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));
        ArgumentException.ThrowIfNullOrEmpty(passwordHash, nameof(passwordHash));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        this.Username = username;
        this.PasswordHash = passwordHash;
        this.Email = email;
    }

    public static User Create(string username, string password, string email) =>
                             new User(username, password, email);

    public void SetPassword(string password)
    {
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));

        this.PasswordHash = password;
    }

    public void SetEmail(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        this.Email = email;
    }

    public void SetUsername(string username)
    {
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));

        this.Username = username;
    }

}