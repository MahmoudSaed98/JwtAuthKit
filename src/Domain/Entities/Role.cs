using Domain.Enums;

namespace Domain.Entities;

public class Role : Entity<int>
{
    public string Name { get; private set; }
    public Permissions Permissions { get; private set; }
    public IReadOnlyList<User> Users { get; private set; } = new List<User>();
    public Role(string name)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(name);
        this.Name = name;
    }
    private Role()  // Called by ef core
    {
    }

    public void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        this.Name = name;
    }

    public void GrantPermission(Permissions permission)
    {
        this.Permissions = (this.Permissions | permission);
    }

    public void RevokePermission(Permissions permissionToRevoke)
    {
        this.Permissions = (this.Permissions & ~permissionToRevoke);
    }
}