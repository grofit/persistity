using System;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Attributes;
using Persistity.Mappings.Mappers;
using Persistity.Registries;
using Persistity.Serialization.Binary;
using Persistity.Serialization.Debug;
using Persistity.Serialization.Json;
using Persistity.Serialization.Xml;
using Tests.Editor.Helpers;
using Tests.Editor.Models;

namespace Tests.Editor
{
    [TestFixture]
    public class SerializationTests
    {
        private IMappingRegistry _mappingRegistry;

        [SetUp]
        public void Setup()
        {
            var mapper = new DefaultTypeMapper();
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
        public void should_serialize_nulled_data_with_debug_serializer()
        {
            var model = SerializationTestHelper.GenerateNulledModel();
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

            var deserializer = new JsonDeserializer(_mappingRegistry);
            var result = deserializer.Deserialize<A>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_json()
        {
            var model = SerializationTestHelper.GenerateNulledModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry);
            var result = deserializer.Deserialize<A>(output);

            SerializationTestHelper.AssertNulledData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_binary()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry);
            var result = deserializer.Deserialize<A>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_binary()
        {
            var model = SerializationTestHelper.GenerateNulledModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry);
            var result = deserializer.Deserialize<A>(output);

            SerializationTestHelper.AssertNulledData(model, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_xml()
        {
            var model = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry);
            var result = deserializer.Deserialize<A>(output);

            SerializationTestHelper.AssertPopulatedData(model, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_xml()
        {
            var model = SerializationTestHelper.GenerateNulledModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry);
            var result = deserializer.Deserialize<A>(output);

            SerializationTestHelper.AssertNulledData(model, result);
        }

        [Test]
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