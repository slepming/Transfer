using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Transfer.Game.Extensions
{
    public static class Hash
    {
        public static string GetHashString(string input)
        {
            if(input == null) return null;
            byte[] hashCode = null;
            using (HashAlgorithm hash = SHA256.Create())
                hashCode =  hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach(byte hash in hashCode)
            {
                sb.Append(hash.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
