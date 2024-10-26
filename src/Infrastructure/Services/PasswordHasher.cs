using Application.Interfaces;
using System.Security.Cryptography;

namespace Infrastructure.Services;

internal class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 10000;
    private readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;
    public string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool Verify(string providedPassword, string hashedPassword)
    {
        string[] parts = hashedPassword.Split(['-']);

        byte[] hash = Convert.FromHexString(parts[0]);

        byte[] salt = Convert.FromHexString(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(providedPassword, salt, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }
}
