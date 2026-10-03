using System.Security.Cryptography;

namespace FromHomeToHome.Backend.Services;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;       // 128-bit Salt
    private const int KeySize = 32;        // 256-bit Hash
    private const int Iterations = 100000; // PBKDF2 Iterations
    private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA256;

    public string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithm,
            KeySize);

        // פורמט מאוחד: Salt:Hash
        return $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        var parts = hashedPassword.Split(':');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] hash = Convert.FromBase64String(parts[1]);

        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithm,
            KeySize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}
