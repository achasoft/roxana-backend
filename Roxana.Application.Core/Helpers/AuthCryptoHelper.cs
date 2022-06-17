using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Roxana.Application.Core.Helpers
{
    public static class AuthCryptoHelper
    {
        public static string GenerateSalt(int length = 32)
        {
            var salt = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

        public static string Hash(string input, string salt)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var bytes = KeyDerivation.Pbkdf2(
                input, saltBytes, KeyDerivationPrf.HMACSHA512, 10000, 16);

            return Convert.ToBase64String(bytes);
        }

        public static bool Verify(string input, string hash, string salt)
        {
            try
            {
                var saltBytes = Convert.FromBase64String(salt);
                var bytes = KeyDerivation.Pbkdf2(input, saltBytes, KeyDerivationPrf.HMACSHA512, 10000, 16);
                var encoded = Convert.ToBase64String(bytes);
                return hash.Equals(encoded);
            }
            catch
            {
                return false;
            }
        }
    }
}