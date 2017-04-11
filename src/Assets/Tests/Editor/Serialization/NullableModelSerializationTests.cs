using System;
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
    public class NullableModelSerializationTests
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
        public void should_serialize_populated_nullable_data_with_debug_serializer()
        {
            var model = SerializationTestHelper.GeneratePopulatedNullableModel();
            var serializer = new DebugSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine(output.AsString);
        }
        
        [Test]
        public void should_correctly_serialize_populated_nullable_data_with_json()
        {
            var model = SerializationTestHelper.GeneratePopulatedNullableModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<NullableTypesModel>(output);

            SerializationTestHelper.AssertNullableModelData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_nullable_data_into_existing_object_with_json()
        {
            var model = SerializationTestHelper.GeneratePopulatedNullableModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = new NullableTypesModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertNullableModelData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_nullable_data_with_binary()
        {
            var model = SerializationTestHelper.GeneratePopulatedNullableModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<NullableTypesModel>(output);

            SerializationTestHelper.AssertNullableModelData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_nullable_data_into_existing_object_with_binary()
        {
            var model = SerializationTestHelper.GeneratePopulatedNullableModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = new NullableTypesModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertNullableModelData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_nullable_data_with_xml()
        {
            var model = SerializationTestHelper.GeneratePopulatedNullableModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<NullableTypesModel>(output);

            SerializationTestHelper.AssertNullableModelData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_nullable_data_into_existing_object_with_xml()
        {
            var model = SerializationTestHelper.GeneratePopulatedNullableModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = new NullableTypesModel();
            deserializer.DeserializeInto(output, result);

            SerializationTestHelper.AssertNullableModelData(model, result);
        }
    }
}