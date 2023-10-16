using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GlobalBilgiQuiz.Helper
{
    public static class CryptographyHelper
    {
        private static readonly SHA1 sha = new SHA1CryptoServiceProvider();

        public static string Encode(string str, string salt)
        {
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(str + salt)));

        }
    }
}
