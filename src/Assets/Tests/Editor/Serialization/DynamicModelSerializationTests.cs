using System;
using NUnit.Framework;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Persistity.Registries;
using Persistity.Serialization.Binary;
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
        public void should_correctly_serialize_dynamic_data_with_json()
        {
            var model = SerializationTestHelper.GeneratePopulatedDynamicTypesModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<DynamicTypesModel>(output);

            SerializationTestHelper.AssertPopulatedDynamicTypesData(model, result);
        }

        [Test]
        public void should_correctly_serialize_dynamic_data_into_existing_object_with_json()
        {
            var model = SerializationTestHelper.GeneratePopulatedDynamicTypesModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var existingInstance = new DynamicTypesModel();
            deserializer.DeserializeInto(output, existingInstance);

            SerializationTestHelper.AssertPopulatedDynamicTypesData(model, existingInstance);
        }

        [Test]
        public void should_correctly_serialize_dynamic_data_with_binary()
        {
            var model = SerializationTestHelper.GeneratePopulatedDynamicTypesModel();
            var serializer = new BinarySerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<DynamicTypesModel>(output);

            SerializationTestHelper.AssertPopulatedDynamicTypesData(model, result);
        }

        [Test]
        public void should_correctly_serialize_dynamic_data_into_existing_object_with_binary()
        {
            var model = SerializationTestHelper.GeneratePopulatedDynamicTypesModel();
            var serializer = new BinarySerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var existingInstance = new DynamicTypesModel();
            deserializer.DeserializeInto(output, existingInstance);

            SerializationTestHelper.AssertPopulatedDynamicTypesData(model, existingInstance);
        }

        [Test]
        public void should_correctly_serialize_dynamic_data_with_xml()
        {
            var model = SerializationTestHelper.GeneratePopulatedDynamicTypesModel();
            var serializer = new XmlSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<DynamicTypesModel>(output);

            SerializationTestHelper.AssertPopulatedDynamicTypesData(model, result);
        }

        [Test]
        public void should_correctly_serialize_dynamic_data_into_existing_object_with_xml()
        {
            var model = SerializationTestHelper.GeneratePopulatedDynamicTypesModel();
            var serializer = new XmlSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var existingInstance = new DynamicTypesModel();
            deserializer.DeserializeInto(output, existingInstance);

            SerializationTestHelper.AssertPopulatedDynamicTypesData(model, existingInstance);
        }

        [Test]
        public void should_correctly_serialize_nulled_dynamic_data_with_json()
        {
            var model = SerializationTestHelper.GenerateNulledDynamicTypesModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<DynamicTypesModel>(output);

            SerializationTestHelper.AsserNulledDynamicTypesData(model, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_dynamic_data_into_existing_object_with_json()
        {
            var model = SerializationTestHelper.GenerateNulledDynamicTypesModel();
            var serializer = new JsonSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new JsonDeserializer(_mappingRegistry, _typeCreator);
            var existingInstance = new DynamicTypesModel();
            deserializer.DeserializeInto(output, existingInstance);

            SerializationTestHelper.AsserNulledDynamicTypesData(model, existingInstance);
        }

        [Test]
        public void should_correctly_serialize_nulled_dynamic_data_with_binary()
        {
            var model = SerializationTestHelper.GenerateNulledDynamicTypesModel();
            var serializer = new BinarySerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<DynamicTypesModel>(output);

            SerializationTestHelper.AsserNulledDynamicTypesData(model, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_dynamic_data_into_existing_object_with_binary()
        {
            var model = SerializationTestHelper.GenerateNulledDynamicTypesModel();
            var serializer = new BinarySerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output.AsBytes));

            var deserializer = new BinaryDeserializer(_mappingRegistry, _typeCreator);
            var existingInstance = new DynamicTypesModel();
            deserializer.DeserializeInto(output, existingInstance);

            SerializationTestHelper.AsserNulledDynamicTypesData(model, existingInstance);
        }

        [Test]
        public void should_correctly_serialize_nulled_dynamic_data_with_xml()
        {
            var model = SerializationTestHelper.GenerateNulledDynamicTypesModel();
            var serializer = new XmlSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var result = deserializer.Deserialize<DynamicTypesModel>(output);

            SerializationTestHelper.AsserNulledDynamicTypesData(model, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_dynamic_data_into_existing_object_with_xml()
        {
            var model = SerializationTestHelper.GenerateNulledDynamicTypesModel();
            var serializer = new XmlSerializer(_mappingRegistry);

            var output = serializer.Serialize(model);
            Console.WriteLine("FileSize: " + output.AsString.Length + " bytes");
            Console.WriteLine(output.AsString);

            var deserializer = new XmlDeserializer(_mappingRegistry, _typeCreator);
            var existingInstance = new DynamicTypesModel();
            deserializer.DeserializeInto(output, existingInstance);

            SerializationTestHelper.AsserNulledDynamicTypesData(model, existingInstance);
        }
    }
}