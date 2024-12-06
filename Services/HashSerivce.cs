using System;
using System.Security.Cryptography;
using System.Text;

namespace ABCDMall.Services
{
    public class HashService
    {
        public static string GetHash(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }
    }
}