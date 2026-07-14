using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedSale_API_Core
{
    internal class Cryptor
    {
        private static byte[] key = { 0x01, 0x02, 0x03, 0x04, 0x05 };

        public static string Encrypt(string password)
        {
            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] cipher = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                cipher[i] = (byte)(data[i] ^ key[i % key.Length]);
            }
            return Encoding.UTF8.GetString(cipher);
        }


        public static string Decrypt(string encryptPassword)
        {
            return Encrypt(encryptPassword);
        }

    }
}
