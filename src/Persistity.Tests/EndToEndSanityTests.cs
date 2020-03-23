using System;
using System.Linq;
using System.Threading.Tasks;
using LazyData;
using LazyData.Binary;
using LazyData.Json;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using LazyData.Xml;
using Persistity.Encryption;
using Persistity.Endpoints.Files;
using Persistity.Endpoints.InMemory;
using Persistity.Pipelines.Builders;
using Persistity.Processors.Encryption;
using Persistity.Tests.Models;
using Persistity.Tests.Pipelines;
using Xunit;
using Assert = Persistity.Tests.Extensions.AssertExtensions;

namespace Persistity.Tests
{
    public class EndToEndSanityTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;

        public EndToEndSanityTests()
        {
            _typeCreator = new TypeCreator();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new DefaultTypeMapper(typeAnalyzer);
            _mappingRegistry = new MappingRegistry(typeMapper);
        }

        [Fact]
        public async void should_correctly_binary_transform_and_save_to_file()
        {
            var filename = "example_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new BinarySerializer(_mappingRegistry);
            var writeFileEndpoint = new FileEndpoint(filename);

            var dummyData = GameData.CreateRandom();
            var output = serializer.Serialize(dummyData);
            
            await writeFileEndpoint.Send(output);
            Console.WriteLine("File Written");         
        }

        [Fact]
        public async void should_correctly_do_a_full_send_then_receive_in_single_pipeline()
        {
            var filename = "example_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new BinarySerializer(_mappingRegistry);
            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var memoryEndpoint = new InMemoryEndpoint();
            var encryptor = new AesEncryptor("some-password");
            var encryptionProcessor = new EncryptDataProcessor(encryptor);
            var decryptionProcessor = new DecryptDataProcessor(encryptor);

            var saveToBinaryFilePipeline = new PipelineBuilder()
                .StartFromInput()
                .SerializeWith(serializer)
                .ProcessWith(encryptionProcessor)
                .ThenSendTo(memoryEndpoint)
                .ThenReceiveFrom(memoryEndpoint)
                .ProcessWith(decryptionProcessor)
                .DeserializeWith(deserializer)
                .Build();

            var dummyData = GameData.CreateRandom();
            var outputModel = await saveToBinaryFilePipeline.Execute(dummyData);
            
            Assert.AreEqual(dummyData, outputModel);
        }

        [Fact]
        public async void should_correctly_fork_stream_for_object_with_builder()
        {
            var expectedString = "hello there some new pipeline";
            var data = "hello";
            var dummyPipeline = new PipelineBuilder()
                .StartFromInput()
                .ThenInvoke(x => Task.FromResult((object) (x + " there")))
                .ThenInvoke(x => Task.FromResult((object) (x + " some")))
                .ThenInvoke(x => Task.FromResult((object) (x + " old")))
                .ThenInvoke(x => Task.FromResult((object) (x + " pipeline")))
                .Build();

            var forkedPipeline = new PipelineBuilder()
                .ForkObjectFrom(dummyPipeline, 2)
                .ThenInvoke(x => Task.FromResult((object) (x + " new")))
                .ThenInvoke(x => Task.FromResult((object) (x + " pipeline")))
                .Build();

            var actualOutput = await forkedPipeline.Execute(data);
            Assert.Equal(expectedString, actualOutput);
        }
        
        [Fact]
        public async void should_correctly_error_if_trying_to_make_data_fork_from_object()
        {
            var dummyPipeline = new PipelineBuilder()
                .StartFromInput()
                .ThenInvoke(x => Task.FromResult((object)"hello"))
                .Build();
            
            Assert.Throws<ArgumentException>(() => new PipelineBuilder().ForkDataFrom(dummyPipeline));
        }
        
        [Fact]
        public async void should_correctly_build_new_pipeline_class()
        {
            var dummyPipeline = new DummyBuiltPipeline();
            Assert.Equal(3, dummyPipeline.Steps.Count());
        }
        
        [Fact]
        public async void should_correctly_error_if_trying_to_make_object_fork_from_data()
        {
            var dummyPipeline = new PipelineBuilder()
                .StartFrom(new InMemoryEndpoint())
                .Build();
            
            Assert.Throws<ArgumentException>(() => new PipelineBuilder().ForkObjectFrom(dummyPipeline));
        }

        [Fact]
        public async void should_correctly_binary_transform_encrypt_save_then_reload()
        {
            var filename = "encrypted_save.bin";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new BinarySerializer(_mappingRegistry);
            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
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
            
            var serializer = new JsonSerializer(_mappingRegistry);
            var writeFileEndpoint = new FileEndpoint(filename);

            var dummyData = GameData.CreateRandom();
            var output = serializer.Serialize(dummyData);
            await writeFileEndpoint.Send(output);
            Console.WriteLine("File Written");
        }

        [Fact]
        public async void should_correctly_json_transform_and_save_to_file()
        {
            var filename = "example_save.json";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new JsonSerializer(_mappingRegistry);
            var writeFileEndpoint = new FileEndpoint(filename);

            var dummyData = GameData.CreateRandom();
            var output = serializer.Serialize(dummyData);

            await writeFileEndpoint.Send(output);
            Console.WriteLine("File Written");
        }

        [Fact]
        public async void should_correctly_xml_transform_and_save_to_file()
        {
            var filename = "example_save.xml";
            Console.WriteLine("{0}/{1}", Environment.CurrentDirectory, filename);
            
            var serializer = new XmlSerializer(_mappingRegistry);
            var writeFileEndpoint = new FileEndpoint(filename);

            var dummyData = GameData.CreateRandom();
            var output = serializer.Serialize(dummyData);

            await writeFileEndpoint.Send(output);
            Console.WriteLine("File Written");
        }
    }
}