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
    public class PopulatedModelSerializationTests
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
        public void should_serialize_populated_data_with_debug_serializer()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();
            var serializer = new DebugSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine(output.AsString);
        }
        
        [Test]
        public void should_correctly_serialize_populated_data_with_json()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }
        
        [Test]
        public void should_correctly_serialize_populated_data_with_binary()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }
        
        [Test]
        public void should_correctly_serialize_populated_data_with_xml()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<ComplexModel>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }
    }
}