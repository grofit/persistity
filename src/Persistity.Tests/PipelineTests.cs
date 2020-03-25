using System;
using System.Linq;
using System.Threading.Tasks;
using LazyData.Binary;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using Persistity.Encryption;
using Persistity.Endpoints.InMemory;
using Persistity.Flow.Builders;
using Persistity.Processors.Encryption;
using Persistity.Tests.Models;
using Persistity.Tests.Pipelines;
using Persistity.Wiretap.Extensions;
using Xunit;
using Assert = Persistity.Tests.Extensions.AssertExtensions;

namespace Persistity.Tests
{
    public class PipelineTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;

        public PipelineTests()
        {
            _typeCreator = new TypeCreator();

            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new DefaultTypeMapper(typeAnalyzer);
            _mappingRegistry = new MappingRegistry(typeMapper);
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
        public async void should_wiretap_correctly()
        {
            var dummyPipeline = new PipelineBuilder()
                .StartFrom(x => Task.FromResult((object)"hello"))
                .ThenInvoke(x => Task.FromResult((object)"there"))
                .Build()
                .AsWireTappable();

            var ranCount = 0;
            dummyPipeline.StartWiretap(1, (x, y) =>
            {
                Assert.Equal("hello", x);
                ranCount++;
            });
            dummyPipeline.StartWiretap(2, (x, y) =>
            {
                Assert.Equal("there", x);
                ranCount++;
            });

            dummyPipeline.Execute();
            Assert.Equal(2, ranCount);
        }
        
        [Fact]
        public async void should_stop_wiretap_correctly()
        {
            var dummyPipeline = new PipelineBuilder()
                .StartFrom(x => Task.FromResult((object)"hello"))
                .ThenInvoke(x => Task.FromResult((object)"there"))
                .Build()
                .AsWireTappable();

            Action<object, object> action = (x, y) => { Assert.True(false); };
            dummyPipeline.StartWiretap(1, action);
            dummyPipeline.StartWiretap(2, action);

            dummyPipeline.StopWiretap(1, action);
            dummyPipeline.StopWiretap(2, action);
            
            dummyPipeline.Execute();
        }
        
        [Fact]
        public async void should_unsubscribe_wiretap_correctly()
        {
            var dummyPipeline = new PipelineBuilder()
                .StartFrom(x => Task.FromResult((object)"hello"))
                .ThenInvoke(x => Task.FromResult((object)"there"))
                .Build()
                .AsWireTappable();

            var sub1 = dummyPipeline.StartWiretap(1, (x, y) =>
            {
                Assert.True(false);
            });
            var sub2 = dummyPipeline.StartWiretap(2, (x, y) =>
            {
                Assert.True(false);
            });

            sub1.Unsubscribe();
            sub2.Unsubscribe();
            
            dummyPipeline.Execute();
        }
    }
}