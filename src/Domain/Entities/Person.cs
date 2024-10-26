namespace Domain.Entities;
public abstract class Person<TPrimaryKey> : Entity<TPrimaryKey>
{
    public string FirstName { get; protected set; } = string.Empty;
    public string LastName { get; protected set; } = string.Empty;
    public string Email { get; protected set; } = string.Empty;

    protected Person(string firstName, string lastName, string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrEmpty(lastName, nameof(lastName));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}