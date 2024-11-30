namespace Application.Interfaces;

public interface IPasswordHasher
{
    public string Hash(string password);

    public bool Verify(string providedPassword, string hashedPassword);
}