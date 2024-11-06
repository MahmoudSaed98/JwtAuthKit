namespace Domain.Entities;

public class Permission : Entity<int>
{
    public string Name { get; private set; } = string.Empty;
    public int Value { get; private set; }

    public Permission(string name, int value) :
        this(default, name, value)
    {
    }
    public Permission(int id, string name, int value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Id = id;
        Name = name;
        Value = value;
    }
    private Permission() { } // Called By EF.

    public void SetName(string name)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);

        this.Name = name;
    }
}