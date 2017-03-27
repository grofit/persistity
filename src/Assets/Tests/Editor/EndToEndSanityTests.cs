using System;
using System.Collections;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Encryption;
using Persistity.Endpoints.Files;
using Persistity.Mappings;
using Persistity.Mappings.Mappers;
using Persistity.Pipelines.Builders;
using Persistity.Processors.Encryption;
using Persistity.Registries;
using Persistity.Serialization.Binary;
using Persistity.Serialization.Json;
using Persistity.Serialization.Xml;
using Persistity.Transformers.Binary;
using Persistity.Transformers.Json;
using Persistity.Transformers.Xml;
using Tests.Editor.Helpers;

namespace Tests.Editor
{
    [TestFixture]
    public class EndToEndSanityTests
    {
        private void HandleError(Exception exception)
        { Assert.Fail(exception.Message); }

        private void HandleSuccess(object data)
        { Console.WriteLine("File Written"); }

        [Test]
        public void should_correctly_binary_transform_and_save_to_file()
        {
            var filename = "example_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);

            var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
            var serializer = new BinarySerializer();
            var deserializer = new BinaryDeserializer();
            var transformer = new BinaryTransformer(serializer, deserializer, mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = transformer.Transform(dummyData);
            writeFileEndpoint.Execute(output, HandleSuccess, HandleError);
        }

        [Test]
        public void should_correctly_transform_encrypt_and_save_to_file_with_builder()
        {
            var filename = "example_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);

            var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
            var serializer = new BinarySerializer();
            var deserializer = new BinaryDeserializer();
            var transformer = new BinaryTransformer(serializer, deserializer, mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);
            var encryptor = new AesEncryptor("some-password");
            var encryptionProcessor = new EncryptDataProcessor(encryptor);

            var saveToBinaryFilePipeline = new PipelineBuilder()
                .TransformWith(transformer)
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

            var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
            var serializer = new BinarySerializer();
            var deserializer = new BinaryDeserializer();
            var transformer = new BinaryTransformer(serializer, deserializer, mappingRegistry);
            var encryptor = new AesEncryptor("dummy-password-123");
            var encryptionProcessor = new EncryptDataProcessor(encryptor);
            var decryptionProcessor = new DecryptDataProcessor(encryptor);
            var writeFileEndpoint = new WriteFileEndpoint(filename);
            var readFileEndpoint = new ReadFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = transformer.Transform(dummyData);
            var encryptedOutput = encryptionProcessor.Process(output);

            writeFileEndpoint.Execute(encryptedOutput, (x) =>
            {
                readFileEndpoint.Execute((data) =>
                {
                    var decryptedData = decryptionProcessor.Process(data);
                    var outputModel = transformer.Transform<A>(decryptedData);
                    SerializationTestHelper.AssertPopulatedData(dummyData, outputModel);
                }, HandleError);
            }, HandleError);
        }

        [Test]
        public void should_correctly_json_transform_and_save_as_binary_to_file()
        {
            var filename = "example_json_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);

            var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
            var serializer = new JsonSerializer();
            var deserializer = new JsonDeserializer();
            var transformer = new JsonTransformer(serializer, deserializer, mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = transformer.Transform(dummyData);
            writeFileEndpoint.Execute(output, HandleSuccess, HandleError);
        }

        [Test]
        public void should_correctly_json_transform_and_save_to_file()
        {
            var filename = "example_save.json";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);

            var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
            var serializer = new JsonSerializer();
            var deserializer = new JsonDeserializer();
            var transformer = new JsonTransformer(serializer, deserializer, mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = transformer.Transform(dummyData);

            writeFileEndpoint.Execute(output, HandleSuccess, HandleError);
        }

        [Test]
        public void should_correctly_xml_transform_and_save_to_file()
        {
            var filename = "example_save.xml";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);

            var mappingRegistry = new MappingRegistry(new DefaultTypeMapper());
            var serializer = new XmlSerializer();
            var deserializer = new XmlDeserializer();
            var transformer = new XmlTransformer(serializer, deserializer, mappingRegistry);
            var writeFileEndpoint = new WriteFileEndpoint(filename);

            var dummyData = SerializationTestHelper.GeneratePopulatedModel();
            var output = transformer.Transform(dummyData);

            writeFileEndpoint.Execute(output, HandleSuccess, HandleError);
        }
    }
}