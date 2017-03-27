using System;
using System.Text;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Mappings;
using Persistity.Mappings.Mappers;
using Persistity.Registries;
using Persistity.Serialization.Binary;
using Persistity.Serialization.Debug;
using Persistity.Serialization.Json;
using Persistity.Serialization.Xml;
using Tests.Editor.Helpers;

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
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var serializer = new DebugSerializer(_mappingRegistry);

            var output = serializer.SerializeData(a);
            Console.WriteLine(output.AsString);
        }

        [Test]
        public void should_serialize_nulled_data_with_debug_serializer()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var serializer = new DebugSerializer(_mappingRegistry);

            var output = serializer.SerializeData(a);
            Console.WriteLine(output.AsString);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_json()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.SerializeData(a);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry);
            var result = deserializer.DeserializeData<A>(output);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_json()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.SerializeData(a);
            Console.WriteLine("FileSize: " + output.AsString + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry);
            var result = deserializer.DeserializeData<A>(output);

            SerializationTestHelper.AssertNulledData(a, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_binary()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.SerializeData(a);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry);
            var result = deserializer.DeserializeData<A>(output);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_binary()
        {
            var a = SerializationTestHelper.GenerateNulledModel();

            var serializer = new BinarySerializer(_mappingRegistry);
            var output = serializer.SerializeData(a);
            Console.WriteLine("FileSize: " + output.AsBytes.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry);
            var result = deserializer.DeserializeData<A>(output);

            SerializationTestHelper.AssertNulledData(a, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_xml()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.SerializeData(a);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry);
            var result = deserializer.DeserializeData<A>(output);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_xml()
        {
            var a = SerializationTestHelper.GenerateNulledModel();

            var serializer = new XmlSerializer(_mappingRegistry);
            var output = serializer.SerializeData(a);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry);
            var result = deserializer.DeserializeData<A>(output);

            SerializationTestHelper.AssertNulledData(a, result);
        }
    }
}