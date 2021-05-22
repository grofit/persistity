using System;
using Persistity.Encryption;
using Persistity.Endpoints.Files;
using Persistity.Extensions;
using Persistity.Processors.Encryption;
using Persistity.Serializers.Json;
using Persistity.Tests.Models;
using Xunit;
using Assert = Persistity.Tests.Extensions.AssertExtensions;

namespace Persistity.Tests
{
    public class EndToEndSanityTests
    {
        public EndToEndSanityTests()
        {
        }

        [Fact]
        public async void should_correctly_save_to_file()
        {
            var filename = "example_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var writeFileEndpoint = new FileEndpoint(filename);
            var serializer = new JsonSerializer();
            
            var dummyData = GameData.CreateRandom();
            var output = serializer.Serialize(dummyData);
            
            await writeFileEndpoint.Send(output);
            Console.WriteLine("File Written");         
        }

        [Fact]
        public async void should_correctly_encrypt_save_then_reload()
        {
            var filename = "encrypted_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new JsonSerializer();
            var deserializer = new JsonDeserializer();
            var encryptor = new AesEncryptor("dummy-password-123");
            var encryptionProcessor = new EncryptDataProcessor(encryptor);
            var decryptionProcessor = new DecryptDataProcessor(encryptor);
            var fileEndpoint = new FileEndpoint(filename);

            var dummyData = GameData.CreateRandom();
            var output = serializer.Serialize(dummyData);
            var encryptedOutput = await encryptionProcessor.Process(output);

            await fileEndpoint.Send(encryptedOutput);
            var data = await fileEndpoint.Receive();
            var decryptedData = await decryptionProcessor.Process(data);
            var outputModel = deserializer.Deserialize<GameData>(decryptedData);
            
            Assert.AreEqual(dummyData, outputModel);
        }

        [Fact]
        public async void should_correctly_json_transform_and_save_as_binary_to_file()
        {
            var filename = "example_json_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new JsonSerializer();
            var writeFileEndpoint = new FileEndpoint(filename);

            var dummyData = GameData.CreateRandom();
            var output = serializer.Serialize(dummyData);
            await writeFileEndpoint.Send(output);
            Console.WriteLine("File Written");
        }
    }
}