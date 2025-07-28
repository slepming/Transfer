using System.Security.Cryptography;
using System.Text;

namespace Transfer.Game.Extensions
{
    /// <summary>
    /// Hashes data, typically used for names that may have incompatible characters
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Hashes input
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Hash string</returns>
        public static string GetHashString(this string input)
        {
            if (input == null) return null;

            byte[] hashCode = null;
            using (HashAlgorithm hash = SHA256.Create())
                hashCode = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();

            foreach (byte hash in hashCode)
            {
                sb.Append(hash.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
