using System;
using System.Linq;
using System.Text;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Mappings;
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
        private TypeMapper _typeMapper = new TypeMapper();
        
        [Test]
        public void should_serialize_populated_data_with_debug_serializer()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new DebugSerializer();

            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine(output);
        }

        [Test]
        public void should_serialize_nulled_data_with_debug_serializer()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new DebugSerializer();

            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine(output);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_json()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new JsonSerializer();
            var jsonOutput = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + Encoding.ASCII.GetByteCount(jsonOutput) + " bytes");
            Console.WriteLine(jsonOutput);
            
            var deserializer = new JsonDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, jsonOutput);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_json()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new JsonSerializer();
            var jsonOutput = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + Encoding.ASCII.GetByteCount(jsonOutput) + " bytes");
            Console.WriteLine(jsonOutput);

            var deserializer = new JsonDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, jsonOutput);

            SerializationTestHelper.AssertNulledData(a, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_binary()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new BinarySerializer();
            var binaryOutput = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + binaryOutput.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(binaryOutput));

            var deserializer = new BinaryDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, binaryOutput);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_binary()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new BinarySerializer();
            var binaryOutput = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + binaryOutput.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(binaryOutput));

            var deserializer = new BinaryDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, binaryOutput);

            SerializationTestHelper.AssertNulledData(a, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_xml()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new XmlSerializer();
            var xmlOutput = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + Encoding.ASCII.GetByteCount(xmlOutput) + " bytes");
            Console.WriteLine(xmlOutput);

            var deserializer = new XmlDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, xmlOutput);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }
    }
}