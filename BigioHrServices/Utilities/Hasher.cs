using System.Security.Cryptography;
using System.Text;

namespace BigioHrServices.Utilities
{
    public class Hasher
    {
        private static string DefaultPassword = "pegawai";
        public string HashString(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            byte[] result = MD5.HashData(bytes);
            return Convert.ToBase64String(result);
        }

        public string HashDefaultPassword()
        {
            return this.HashString(DefaultPassword);
        }
    }
}
