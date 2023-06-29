using System;
using System.Security.Cryptography;
using System.Text;

namespace CRYPTO_Library
{
    internal static class Cryptograph
    {
        private static string _key;
        private static byte[] _keyArray;

        public static string Key
        {
            set
            {
                
                _key = value;
                _keyArray = UTF8Encoding.UTF8.GetBytes(_key);
            }
        }
        public static string Encrypt(string str)
        {
            ICryptoTransform transform = GetAES().CreateEncryptor();
            byte[] strBytes = UTF8Encoding.UTF8.GetBytes(str);
            byte[] result = transform.TransformFinalBlock(strBytes, 0, strBytes.Length);
            return Convert.ToBase64String(result,0,result.Length);
        }
        public static string Decrypt(string str)
        {
            ICryptoTransform transform = GetAES().CreateDecryptor();
            byte[] strBytes = Convert.FromBase64String(str);
            byte[] result = transform.TransformFinalBlock(strBytes, 0, strBytes.Length);
            return Encoding.UTF8.GetString(result);
        }

        private static Aes GetAES()
        {
            Aes aes = Aes.Create();

            aes.Key = _keyArray;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            return aes;
        }
    }
}
