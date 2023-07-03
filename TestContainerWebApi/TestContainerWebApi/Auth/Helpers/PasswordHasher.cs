using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace TestContainerWebApi.Auth.Helpers
{
    public static class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            // Generate a 128-bit salt using a secure PRNG (Pseudo-Random Number Generator)
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password using PBKDF2 with 10000 iterations
            string hashedPassword = GetHash(password, salt);

            // Combine the salt and hashed password for storage
            string combinedHash = $"$HASH_PBKDF2v2$V1${Convert.ToBase64String(salt)}${hashedPassword}";

            return combinedHash;
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            // Extract the salt and hash from the stored hash string
            string[] hashParts = storedHash.Split('$');
            string saltString = hashParts[3];
            string hashedPassword = hashParts[4];

            byte[] salt = Convert.FromBase64String(saltString);

            // Hash the entered password using the extracted salt
            string enteredPasswordHash = GetHash(password, salt);

            // Compare the generated hash with the stored hash
            bool passwordMatches = enteredPasswordHash.Equals(hashedPassword);

            return passwordMatches;
        }

        private static string GetHash(string password, byte[] salt)
        {
            // Hash the password using PBKDF2 with 10000 iterations
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));

            return hashedPassword;
        }
    }
}
