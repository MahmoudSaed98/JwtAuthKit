using Domain.Enums;

namespace Domain.Entities;

public class Role : Enumeration<Role>
{

    public readonly static Role Registered = new Role(1, "Registered");

    public readonly static Role Admin = new Role(2, "Admin");

    public ICollection<User> Users { get; private set; } = new List<User>();
    public ICollection<Permission> Permissions { get; private set; } = new List<Permission>();
    public Role(int value, string name)
        : base(value, name)
    {
    }

    private Role() { }
}