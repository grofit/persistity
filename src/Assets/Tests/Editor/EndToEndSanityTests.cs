using System;
using System.Collections;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Encryption;
using Persistity.Endpoints.Files;
using Persistity.Mappings;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Pipelines.Builders;
using Persistity.Processors.Encryption;
using Persistity.Registries;
using Persistity.Serialization.Binary;
using Persistity.Serialization.Json;
using Persistity.Serialization.Xml;
using Tests.Editor.Helpers;
using Tests.Editor.Models;

namespace Tests.Editor
{
    [TestFixture]
    public class EndToEndSanityTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;

        [SetUp]
        public void Setup()
        {
            _typeCreator = new TypeCreator();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new DefaultTypeMapper(typeAnalyzer);
            _mappingRegistry = new MappingRegistry(typeMapper);
        }

        private void HandleError(Exception exception)
        { Assert.Fail(exception.Message); }

        private void HandleSuccess(object data)
        { Console.WriteLine("File Written"); }

        [Test]
        public void should_correctly_binary_transform_and_save_to_file()
        {
            var filename = "example_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new BinarySerializer(_mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = serializer.Serialize(dummyData);
            writeFileEndpoint.Execute(output, HandleSuccess, HandleError);
        }

        [Test]
        public void should_correctly_transform_encrypt_and_save_to_file_with_builder()
        {
            var filename = "example_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new BinarySerializer(_mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);
            var encryptor = new AesEncryptor("some-password");
            var encryptionProcessor = new EncryptDataProcessor(encryptor);

            var saveToBinaryFilePipeline = new PipelineBuilder()
                .SerializeWith(serializer)
                .ProcessWith(encryptionProcessor)
                .SendTo(writeFileEndpoint)
                .Build();

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            saveToBinaryFilePipeline.Execute(dummyData, HandleSuccess, HandleError);
        }

        [Test]
        public void should_correctly_binary_transform_encrypt_save_then_reload()
        {
            var filename = "encrypted_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new BinarySerializer(_mappingRegistry);
            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var encryptor = new AesEncryptor("dummy-password-123");
            var encryptionProcessor = new EncryptDataProcessor(encryptor);
            var decryptionProcessor = new DecryptDataProcessor(encryptor);
            var writeFileEndpoint = new WriteFileEndpoint(filename);
            var readFileEndpoint = new ReadFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = serializer.Serialize(dummyData);
            var encryptedOutput = encryptionProcessor.Process(output);

            writeFileEndpoint.Execute(encryptedOutput, (x) =>
            {
                readFileEndpoint.Execute((data) =>
                {
                    var decryptedData = decryptionProcessor.Process(data);
                    var outputModel = (ComplexModel)deserializer.Deserialize(decryptedData);
                    SerializationTestHelper.AssertPopulatedData(dummyData, outputModel);
                }, HandleError);
            }, HandleError);
        }

        [Test]
        public void should_correctly_json_transform_and_save_as_binary_to_file()
        {
            var filename = "example_json_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new JsonSerializer(_mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = serializer.Serialize(dummyData);
            writeFileEndpoint.Execute(output, HandleSuccess, HandleError);
        }

        [Test]
        public void should_correctly_json_transform_and_save_to_file()
        {
            var filename = "example_save.json";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new JsonSerializer(_mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = serializer.Serialize(dummyData);

            writeFileEndpoint.Execute(output, HandleSuccess, HandleError);
        }

        [Test]
        public void should_correctly_xml_transform_and_save_to_file()
        {
            var filename = "example_save.xml";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new XmlSerializer(_mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = serializer.Serialize(dummyData);

            writeFileEndpoint.Execute(output, HandleSuccess, HandleError);
        }
    }
}