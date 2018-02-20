using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Persistity.Encryption
{
    public class AesEncryptor : IEncryptor
    {
        private RandomNumberGenerator _random;
        
        public int KeySize { get; }
        public int Iterations { get; }
        public string Password { get; }

        public AesEncryptor(string password, int keySize = 128, int iterations = 1000)
        {
            Password = password;
            KeySize = keySize;
            Iterations = iterations;

            _random = RandomNumberGenerator.Create();
        }

        public byte[] Encrypt(byte[] data)
        {
            var saltStringBytes = GetRandomData(128);
            var ivStringBytes = GetRandomData(128);
            var password = new Rfc2898DeriveBytes(Password, saltStringBytes, Iterations);
            var keyBytes = password.GetBytes(KeySize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.ISO10126;
                using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(data, 0, data.Length);
                            cryptoStream.FlushFinalBlock();

                            var cipherTextBytes = saltStringBytes;
                            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                            return cipherTextBytes;
                        }
                    }
                }
            }
        }

        public byte[] Decrypt(byte[] data)
        {
            var saltStringBytes = data.Take(KeySize / 8).ToArray();
            var ivStringBytes = data.Skip(KeySize / 8).Take(KeySize / 8).ToArray();
            var cipherTextBytes = data.Skip((KeySize / 8) * 2).Take(data.Length - ((KeySize / 8) * 2)).ToArray();

            var password = new Rfc2898DeriveBytes(Password, saltStringBytes, Iterations);
            var keyBytes = password.GetBytes(KeySize / 8);
            using (var symmetricKey = new RijndaelManaged())
            {
                symmetricKey.BlockSize = 128;
                symmetricKey.Mode = CipherMode.CBC;
                symmetricKey.Padding = PaddingMode.ISO10126;
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                {
                    using (var memoryStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            var plainTextBytes = new byte[cipherTextBytes.Length];
                            var readCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                            return plainTextBytes.Take(readCount).ToArray();
                        }
                    }
                }
            }
        }
       
        private byte[] GetRandomData(int bits)
        {
            var result = new byte[bits / 8];
            _random.GetBytes(result);
            return result;
        }
    }
}