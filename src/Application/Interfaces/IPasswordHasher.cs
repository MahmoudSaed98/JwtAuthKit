namespace Application.Interfaces;

public interface IPasswordHasher
{
    /// <summary>
    /// Generates a hashed password from the provided plain text password.
    /// </summary>
    /// <param name="password">The plain text password.</param>
    /// <returns>The hashed password.</returns>
    public string Hash(string password);


    /// <summary>
    /// Verifies if the provided plain text password matches the hashed password.
    /// </summary>
    /// <param name="providedPassword">The plain text password provided by the user.</param>
    ///     /// <param name="hashedPassword">The stored hashed password.</param>
    /// <returns>True if the password is correct, otherwise false.</returns>
    /// 
    public bool Verify(string providedPassword, string hashedPassword);
}