using System;
using System.Text;
using System.Security.Cryptography;

namespace K12.Data.Utility
{
    public class PasswordHash
    {
        public static string Compute(string password)
        {
            SHA1Managed sha1 = new SHA1Managed();
            Encoding utf8 = Encoding.UTF8;

            byte[] hashResult = sha1.ComputeHash(utf8.GetBytes(password));

            return Convert.ToBase64String(hashResult);
        }
    }
}
