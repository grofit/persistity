using System;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization.Binary;
using Persistity.Serialization.Debug;
using Persistity.Serialization.Json;
using Persistity.Serialization.Xml;
using Tests.Editor.Helpers;
using Tests.Editor.Models;

namespace Tests.Editor.Serialization
{
    [TestFixture]
    public class DynamicModelSerializationTests
    {
        private IMappingRegistry _mappingRegistry;

        [SetUp]
        public void Setup()
        {
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

            var deserializer = new JsonDeserializer(_mappingRegistry);
            var result = deserializer.Deserialize<DynamicTypesModel>(output);

            SerializationTestHelper.AssertDynamicTypesData(model, result);
        }
    }
}