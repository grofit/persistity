using System;
using System.Text;
using Persistity.Encryption;
using Xunit;

namespace Tests.Editor
{
    public class EncryptionTests
    {
        [Fact]
        public void should_correctly_encrypt_and_decrypt_data()
        {

            var expectedBytes = Encoding.UTF8.GetBytes("This is what was requested");
            Console.WriteLine("default: {0}", BitConverter.ToString(expectedBytes));

            var encryptor = new AesEncryptor("some-password");

            var encryptedData = encryptor.Encrypt(expectedBytes);
            Console.WriteLine("encrypted: {0}", BitConverter.ToString(encryptedData));

            var decryptedData = encryptor.Decrypt(encryptedData);
            Console.WriteLine("decrypted: {0}", BitConverter.ToString(decryptedData));

            Assert.Equal(expectedBytes, decryptedData);
        }
    }
}