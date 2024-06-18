using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class HashService
    {
        public static string GetHash(string key) =>
            BitConverter.ToString(SHA256.HashData(Encoding.UTF8.GetBytes(key))).ToLower();
    }
}
