using System;
using System.Text;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Mappings;
using Persistity.Mappings.Mappers;
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
        private DefaultTypeMapper _defaultTypeMapper = new DefaultTypeMapper();
        
        [Test]
        public void should_serialize_populated_data_with_debug_serializer()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var typeStuff = _defaultTypeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new DebugSerializer();

            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine(Encoding.Default.GetString(output));
        }

        [Test]
        public void should_serialize_nulled_data_with_debug_serializer()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var typeStuff = _defaultTypeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new DebugSerializer();

            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine(Encoding.Default.GetString(output));
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_json()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var typeStuff = _defaultTypeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new JsonSerializer();
            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + output.Length + " bytes");
            Console.WriteLine(Encoding.Default.GetString(output));

            var deserializer = new JsonDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, output);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_json()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var typeStuff = _defaultTypeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new JsonSerializer();
            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + output.Length + " bytes");
            Console.WriteLine(Encoding.Default.GetString(output));

            var deserializer = new JsonDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, output);

            SerializationTestHelper.AssertNulledData(a, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_binary()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var typeStuff = _defaultTypeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new BinarySerializer();
            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + output.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output));

            var deserializer = new BinaryDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, output);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_binary()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var typeStuff = _defaultTypeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new BinarySerializer();
            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + output.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(output));

            var deserializer = new BinaryDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, output);

            SerializationTestHelper.AssertNulledData(a, result);
        }

        [Test]
        public void should_correctly_serialize_populated_data_with_xml()
        {
            var a = SerializationTestHelper.GeneratePopulatedModel();
            var typeStuff = _defaultTypeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new XmlSerializer();
            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + output.Length + " bytes");
            Console.WriteLine(Encoding.Default.GetString(output));

            var deserializer = new XmlDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, output);

            SerializationTestHelper.AssertPopulatedData(a, result);
        }

        [Test]
        public void should_correctly_serialize_nulled_data_with_xml()
        {
            var a = SerializationTestHelper.GenerateNulledModel();
            var typeStuff = _defaultTypeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new XmlSerializer();
            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + output.Length + " bytes");
            Console.WriteLine(Encoding.Default.GetString(output));

            var deserializer = new XmlDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, output);

            SerializationTestHelper.AssertNulledData(a, result);
        }
    }
}