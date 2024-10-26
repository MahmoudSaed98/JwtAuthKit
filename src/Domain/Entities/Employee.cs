namespace Domain.Entities;

public class Employee : Person<int>
{
    public decimal Salary { get; private set; }
    private Employee(string firstName, string lastName, string email, decimal salary)
        : base(firstName, lastName, email)
    {
        this.Salary = salary;
    }

    public static Employee Create(string firstname, string lastName, string email, decimal salary) =>
                                  new(firstname, lastName, email, salary);
    public void Update(string firstname, string lastName, string email, decimal salary)
    {
        ArgumentException.ThrowIfNullOrEmpty(firstname, nameof(firstname));
        ArgumentException.ThrowIfNullOrEmpty(lastName, nameof(lastName));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));

        this.FirstName = firstname;
        this.LastName = lastName;
        this.Email = email;
        this.Salary = salary;
    }
}