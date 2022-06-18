using System.Security.Cryptography;
using System.Text;

namespace Roxana.Application.Core.Helpers
{
    public static class CryptoHelper
    {
        private const int DerivationIterations = 1000;
        private const string ValidKeys = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string EncryptPassword(string password, string salt)
        {
            var bytes = Encoding.UTF8.GetBytes(salt);
            return ComputeHash(password, "SHA256", bytes);
        }
        public static bool VerifyPassword(string password, string hash)
        {
            return VerifyHash(password, "SHA256", hash);
        }
        
        public static string ComputeHash(string plainText, string hashAlgorithm, byte[] saltBytes)
        {
            // Salt size
            saltBytes = new byte[8];

            // Convert Text to Array
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            // Create Array
            byte[] plainTextSaltBytes =
                new byte[plainTextBytes.Length + saltBytes.Length];

            // Copy Text into Array
            for (int i = 0; i < plainTextBytes.Length; i++)
                plainTextSaltBytes[i] = plainTextBytes[i];

            // Append Salt
            for (int i = 0; i < saltBytes.Length; i++)
                plainTextSaltBytes[plainTextBytes.Length + i] = saltBytes[i];

            // Algorithm
            HashAlgorithm hash;

            // Initialize Class
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hash = new SHA1Managed();
                    break;

                case "SHA256":
                    hash = new SHA256Managed();
                    break;

                case "SHA384":
                    hash = new SHA384Managed();
                    break;

                case "SHA512":
                    hash = new SHA512Managed();
                    break;

                default:
                    hash = new MD5CryptoServiceProvider();
                    break;
            }

            byte[] hashBytes = hash.ComputeHash(plainTextSaltBytes);

            byte[] hashSaltBytes = new byte[hashBytes.Length +
                                            saltBytes.Length];

            // Hash to Array
            for (int i = 0; i < hashBytes.Length; i++)
                hashSaltBytes[i] = hashBytes[i];

            // Append Salt
            for (int i = 0; i < saltBytes.Length; i++)
                hashSaltBytes[hashBytes.Length + i] = saltBytes[i];

            // Convert result into a base64-encoded string.
            string hashValue = Convert.ToBase64String(hashSaltBytes);

            // Return Result
            return hashValue;
        }

        public static bool VerifyHash(string plainText,
            string hashAlgorithm,
            string hashValue)
        {
            // Base64-encoded hash
            byte[] hashWithSaltBytes = Convert.FromBase64String(hashValue);

            // Hash without Salt
            int hashSizeInBits, hashSizeInBytes;

            // Size of hash is based on the specified algorithm.
            switch (hashAlgorithm.ToUpper())
            {
                case "SHA1":
                    hashSizeInBits = 160;
                    break;

                case "SHA256":
                    hashSizeInBits = 256;
                    break;

                case "SHA384":
                    hashSizeInBits = 384;
                    break;

                case "SHA512":
                    hashSizeInBits = 512;
                    break;

                default: // MD5
                    hashSizeInBits = 128;
                    break;
            }

            // Convert to bytes.
            hashSizeInBytes = hashSizeInBits / 8;

            // Verify Hash Length
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            // Array to hold Salt
            byte[] saltBytes = new byte[hashWithSaltBytes.Length -
                                        hashSizeInBytes];

            // Salt to New Array
            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = hashWithSaltBytes[hashSizeInBytes + i];

            string expectedHashString =
                ComputeHash(plainText, hashAlgorithm, saltBytes);

            return (hashValue == expectedHashString);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string CreateSalt(int size)
        {
            var rng = new RNGCryptoServiceProvider();
            var buff = new byte[size];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        public static int CreateTokenStoreCode()
        {
            return new Random().Next(1000000, 10000000);
        }

        public static string DecryptRijndael(string cipherText, string key, int blockSize = 128)
        {
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(blockSize / 8).ToArray();
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(blockSize / 8).Take(blockSize / 8).ToArray();
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip(blockSize / 8 * 2)
                .Take(cipherTextBytesWithSaltAndIv.Length - blockSize / 8 * 2).ToArray();

            using (var password = new Rfc2898DeriveBytes(key, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(blockSize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = blockSize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        public static string DecryptSHA512(string encryptedText, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have valid value.", nameof(key));
            if (string.IsNullOrEmpty(encryptedText))
                throw new ArgumentException("The encrypted text must have valid value.", nameof(encryptedText));

            var combined = Convert.FromBase64String(encryptedText);
            var buffer = new byte[combined.Length];
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));

                aes.Key = aesKey;

                var iv = new byte[aes.IV.Length];
                var ciphertext = new byte[buffer.Length - iv.Length];

                Array.ConstrainedCopy(combined, 0, iv, 0, iv.Length);
                Array.ConstrainedCopy(combined, iv.Length, ciphertext, 0, ciphertext.Length);

                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(ciphertext))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    return Encoding.UTF8.GetString(resultStream.ToArray());
                }
            }
        }

        public static string EncryptRijndael(string plainText, string key, int blockSize = 128)
        {
            var saltStringBytes = Generate256BitsOfRandomEntropy(blockSize);
            var ivStringBytes = Generate256BitsOfRandomEntropy(blockSize);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(key, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(blockSize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = blockSize;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string EncryptSHA512(string text, string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have valid value.", nameof(key));
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("The text must have valid value.", nameof(text));

            var buffer = Encoding.UTF8.GetBytes(text);
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(Encoding.UTF8.GetBytes(key)), 0, aesKey, 0, 24);

            using (var aes = Aes.Create())
            {
                if (aes == null)
                    throw new ArgumentException("Parameter must not be null.", nameof(aes));

                aes.Key = aesKey;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var resultStream = new MemoryStream())
                {
                    using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
                    using (var plainStream = new MemoryStream(buffer))
                    {
                        plainStream.CopyTo(aesStream);
                    }

                    var result = resultStream.ToArray();
                    var combined = new byte[aes.IV.Length + result.Length];
                    Array.ConstrainedCopy(aes.IV, 0, combined, 0, aes.IV.Length);
                    Array.ConstrainedCopy(result, 0, combined, aes.IV.Length, result.Length);

                    return Convert.ToBase64String(combined);
                }
            }
        }

        public static string GeneratePhoneConfirmationCode()
        {
            return new Random().Next(0, 999999).ToString("D6");
        }

        public static string GetShortUniqueId()
        {
            return Guid.NewGuid().ToShortUniqueId();
        }

        public static string GetUniqueKey(int maxSize)
        {
            var chars = ValidKeys.ToCharArray();
            var data = new byte[1];
            using (var crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[maxSize];
                crypto.GetNonZeroBytes(data);
            }

            var result = new StringBuilder(maxSize);
            foreach (var b in data) result.Append(chars[b % chars.Length]);

            return result.ToString();
        }

        public static string Md5(string input)
        {
            using (var hasher = MD5.Create())
            {
                var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
            }
        }

        public static string Sha1(string input)
        {
            using (var hasher = new SHA1Managed())
            {
                var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
                return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy(int blockSize = 256)
        {
            var randomBytes = new byte[blockSize / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(randomBytes);
            }

            return randomBytes;
        }
    }
}