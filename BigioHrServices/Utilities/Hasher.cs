using System.Security.Cryptography;
using System.Text;

namespace BigioHrServices.Utilities
{
    public class Hasher
    {
        private static string DefaultPassword = "pegawai";
        private static string DefaultPIN = "101010";
        public string HashString(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            byte[] result = MD5.HashData(bytes);
            return Convert.ToBase64String(result);
        }

        public static string HashString2(string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            byte[] result = MD5.HashData(bytes);
            return Convert.ToBase64String(result);
        }

        public string HashDefaultPassword()
        {
            return this.HashString(DefaultPassword);
        }

        public string HashDefaultPIN()
        {
            return this.HashString(DefaultPIN);
        }

        public bool verifiyPassword(string Password, string HashedPassword)
        {
            return HashedPassword == HashString(Password);
        }

        public bool verivyPIN(string PIN, string HashedPIN)
        {
            return HashedPIN == HashString(PIN);
        }
    }
}
