namespace Domain.Entities;

public class Permission : Entity<int>
{
    public string Name { get; private set; } = string.Empty;
    public ICollection<Role> Roles { get; private set; } = new List<Role>();

    public Permission(string name) : this(default, name)
    { }
    public Permission(int id, string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Id = id;
        Name = name;
    }
    private Permission() { } // Called By EF.
}