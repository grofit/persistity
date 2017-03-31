using System;
using NUnit.Framework;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization.Json;
using Tests.Editor.Helpers;
using Tests.Editor.Models;

namespace Tests.Editor.Serialization
{
    [TestFixture]
    public class DynamicModelSerializationTests
    {
        private IMappingRegistry _mappingRegistry;
        private ITypeCreator _typeCreator;

        [SetUp]
        public void Setup()
        {
            _typeCreator = new TypeCreator();

            var analyzer = new TypeAnalyzer();
            var mapper = new DefaultTypeMapper(analyzer);
            _mappingRegistry = new MappingRegistry(mapper);
        }
        
        [Test]
        [Ignore]
        public void should_correctly_serialize_dynamic_data_with_json()
        {
            var model = SerializationTestHelper.GenerateDynamicTypesModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<DynamicTypesModel>(output);

            SerializationTestHelper.AssertDynamicTypesData(model, result);
        }
    }
}