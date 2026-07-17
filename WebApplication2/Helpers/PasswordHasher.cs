using Isopoh.Cryptography.Argon2;

namespace MyApi.Helpers; // Make sure this namespace matches where you place the file

public static class PasswordHasher
{
    public static string HashPassword(string rawPassword)
    {
        return Argon2.Hash(rawPassword);
    }

    public static bool VerifyPassword(string rawPassword, string storedHash)
    {
        return Argon2.Verify(storedHash, rawPassword);
    }
}