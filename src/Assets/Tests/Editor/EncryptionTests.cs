using System;
using System.Text;
using NUnit.Framework;
using Persistity.Encryption;

namespace Tests.Editor
{
    [TestFixture]
    public class EncryptionTests
    {
        [Test]
        public void should_correctly_encrypt_and_decrypt_data()
        {

            var expectedBytes = Encoding.UTF8.GetBytes("This is what was requested");
            Console.WriteLine("default: {0}", BitConverter.ToString(expectedBytes));

            var encryptor = new AesEncryptor("some-password");

            var encryptedData = encryptor.Encrypt(expectedBytes);
            Console.WriteLine("encrypted: {0}", BitConverter.ToString(encryptedData));

            var decryptedData = encryptor.Decrypt(encryptedData);
            Console.WriteLine("decrypted: {0}", BitConverter.ToString(decryptedData));

            CollectionAssert.AreEqual(decryptedData, expectedBytes);
        }
    }
}