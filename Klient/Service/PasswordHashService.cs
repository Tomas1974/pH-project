using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace service.Services;

public class PasswordHashService
{
    
    /*
     * Hashes a password using Argon2id key derivation function with specified parameters.
     */
    public  string HashPassword(string password, string salt)
    {
        using var hashAlgo = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = Convert.FromBase64String(salt),
            MemorySize = 12288,
            Iterations = 3,
            DegreeOfParallelism = 1,
        };
        return Convert.ToBase64String(hashAlgo.GetBytes(256));
    }

    /*
     * Verifies a hashed password by comparing it to the newly hashed input password.
     */
    public  bool VerifyHashedPassword(string password, string hash, string salt)
    {
        return HashPassword(password, salt).SequenceEqual(hash);
    }
    
    /*
     * Generates a random salt using a secure random number generator.
     */
    public string GenerateSalt()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(128));
    }
}