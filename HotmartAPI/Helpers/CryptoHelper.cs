using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HotmartAPI.Helpers
{
    public static class CryptoHelper
    {
        private static int _iterations;

        private static int _keySize;

        private static string _hash;

        private static string _salt;

        private static string _vector;

        static CryptoHelper()
        {
            _iterations = 2;
            _keySize = 256;
            _hash = "SHA1";
            _salt = "astr7ias38490a98";
            _vector = "8947az34zyl34kjq";
        }

        public static string Decrypt(string value, string password)
        {
            return Decrypt<AesManaged>(value, password);
        }

        public static string Decrypt<T>(string value, string password)
        where T : SymmetricAlgorithm, new()
        {
            byte[] numArray;
            string str;
            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(_vector);
                byte[] bytes1 = Encoding.ASCII.GetBytes(_salt);
                byte[] numArray1 = Convert.FromBase64String(value);
                int num = 0;
                T t = Activator.CreateInstance<T>();
                try
                {
                    PasswordDeriveBytes passwordDeriveByte = new PasswordDeriveBytes(password, bytes1, _hash, _iterations);
                    byte[] bytes2 = passwordDeriveByte.GetBytes(_keySize / 8);
                    t.Mode = CipherMode.CBC;
                    using (ICryptoTransform cryptoTransform = t.CreateDecryptor(bytes2, bytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(numArray1))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                            {
                                numArray = new byte[(int)numArray1.Length];
                                num = cryptoStream.Read(numArray, 0, (int)numArray.Length);
                            }
                        }
                    }
                    t.Clear();
                }
                finally
                {
                    if (t != null)
                    {
                        ((IDisposable)(object)t).Dispose();
                    }
                }
                str = Encoding.UTF8.GetString(numArray, 0, num);
            }
            catch (Exception)
            {
                str = string.Empty;
            }
            return str;
        }

        public static string DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if ((cipherText == null ? true : cipherText.Length == 0))
            {
                throw new ArgumentNullException("cipherText");
            }
            if ((Key == null ? true : Key.Length == 0))
            {
                throw new ArgumentNullException("Key");
            }
            if ((IV == null ? true : IV.Length == 0))
            {
                throw new ArgumentNullException("Key");
            }
            string end = null;
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Key = Key;
                rijndaelManaged.IV = IV;
                rijndaelManaged.Padding = PaddingMode.None;
                ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream memoryStream = new MemoryStream(cipherText))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            end = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return end;
        }

        public static string Encrypt(string value, string password)
        {
            return Encrypt<AesManaged>(value, password);
        }

        public static string Encrypt<T>(string value, string password)
        where T : SymmetricAlgorithm, new()
        {
            byte[] array;
            byte[] bytes = Encoding.ASCII.GetBytes(_vector);
            byte[] numArray = Encoding.ASCII.GetBytes(_salt);
            byte[] bytes1 = Encoding.UTF8.GetBytes(value);
            T t = Activator.CreateInstance<T>();
            try
            {
                PasswordDeriveBytes passwordDeriveByte = new PasswordDeriveBytes(password, numArray, _hash, _iterations);
                byte[] numArray1 = passwordDeriveByte.GetBytes(_keySize / 8);
                t.Mode = CipherMode.CBC;
                using (ICryptoTransform cryptoTransform = t.CreateEncryptor(numArray1, bytes))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(bytes1, 0, (int)bytes1.Length);
                            cryptoStream.FlushFinalBlock();
                            array = memoryStream.ToArray();
                        }
                    }
                }
                t.Clear();
            }
            finally
            {
                if (t != null)
                {
                    ((IDisposable)(object)t).Dispose();
                }
            }
            return Convert.ToBase64String(array);
        }
    }
}
