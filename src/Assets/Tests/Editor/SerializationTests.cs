using System;
using System.Linq;
using System.Text;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Mappings;
using Persistity.Serialization;
using Tests.Editor.Models;
using UnityEngine;

namespace Tests.Editor
{
    [TestFixture]
    public class SerializationTests
    {
        private TypeMapper _typeMapper = new TypeMapper();

        private A GenerateDummyData()
        {
            var a = new A();
            a.TestValue = "WOW";
            a.NonPersisted = 100;
            a.Stuff.Add("woop");
            a.Stuff.Add("poow");

            a.NestedValue = new B
            {
                IntValue = 10,
                StringValue = "Hello",
                NestedArray = new[] {new C {FloatValue = 2.43f}}
            };

            a.NestedArray = new B[2];
            a.NestedArray[0] = new B
            {
                IntValue = 20,
                StringValue = "There",
                NestedArray = new[] { new C { FloatValue = 3.5f } }
            };

            a.NestedArray[1] = new B
            {
                IntValue = 30,
                StringValue = "Sir",
                NestedArray = new[]
                {
                     new C { FloatValue = 4.1f },
                     new C { FloatValue = 5.2f }
                }
            };

            a.AllTypes = new D
            {
                ByteValue = byte.MaxValue,
                ShortValue = short.MaxValue,
                IntValue = int.MaxValue,
                LongValue = long.MaxValue,
                GuidValue = Guid.NewGuid(),
                DateTimeValue = DateTime.MaxValue,
                Vector2Value = Vector2.one,
                Vector3Value = Vector3.one,
                Vector4Value = Vector4.one,
                QuaternionValue = new Quaternion(1.0f, 1.0f, 1.0f, 1.0f)
            };

            a.SimpleDictionary.Add("key1", "some-value");
            a.SimpleDictionary.Add("key2", "some-other-value");

            a.ComplexDictionary.Add(new E{ IntValue = 10}, new C{ FloatValue = 32.2f });
            //a.ComplexDictionary.Add(new E{ IntValue = 20}, null);

            return a;
        }

        private void AssertionOnDummyData(A expected, A result)
        {
            Assert.That(result.TestValue, Is.EqualTo(expected.TestValue));
            Assert.That(result.NonPersisted, Is.EqualTo(0));
            Assert.That(result.NestedValue, Is.Not.Null);
            Assert.That(result.NestedValue.IntValue, Is.EqualTo(expected.NestedValue.IntValue));
            Assert.That(result.NestedValue.StringValue, Is.EqualTo(expected.NestedValue.StringValue));
            Assert.That(result.NestedValue.NestedArray, Is.Not.Null);
            Assert.That(result.NestedValue.NestedArray.Length, Is.EqualTo(expected.NestedValue.NestedArray.Length));
            Assert.That(result.NestedValue.NestedArray[0].FloatValue, Is.EqualTo(expected.NestedValue.NestedArray[0].FloatValue));
            Assert.That(result.NestedArray, Is.Not.Null);
            Assert.That(result.NestedArray.Length, Is.EqualTo(expected.NestedArray.Length));
            Assert.That(result.NestedArray[0].IntValue, Is.EqualTo(expected.NestedArray[0].IntValue));
            Assert.That(result.NestedArray[0].StringValue, Is.EqualTo(expected.NestedArray[0].StringValue));
            Assert.That(result.NestedArray[0].NestedArray, Is.Not.Null);
            Assert.That(result.NestedArray[0].NestedArray.Length, Is.EqualTo(expected.NestedArray[0].NestedArray.Length));
            Assert.That(result.NestedArray[0].NestedArray[0].FloatValue, Is.EqualTo(expected.NestedArray[0].NestedArray[0].FloatValue));
            Assert.That(result.NestedArray[1].IntValue, Is.EqualTo(expected.NestedArray[1].IntValue));
            Assert.That(result.NestedArray[1].StringValue, Is.EqualTo(expected.NestedArray[1].StringValue));
            Assert.That(result.NestedArray[1].NestedArray, Is.Not.Null);
            Assert.That(result.NestedArray[1].NestedArray.Length, Is.EqualTo(expected.NestedArray[1].NestedArray.Length));
            Assert.That(result.NestedArray[1].NestedArray[0].FloatValue, Is.EqualTo(expected.NestedArray[1].NestedArray[0].FloatValue));
            Assert.That(result.NestedArray[1].NestedArray[1].FloatValue, Is.EqualTo(expected.NestedArray[1].NestedArray[1].FloatValue));
            Assert.That(result.AllTypes, Is.Not.Null);
            Assert.That(result.AllTypes.ByteValue, Is.EqualTo(expected.AllTypes.ByteValue));
            Assert.That(result.AllTypes.ShortValue, Is.EqualTo(expected.AllTypes.ShortValue));
            Assert.That(result.AllTypes.IntValue, Is.EqualTo(expected.AllTypes.IntValue));
            Assert.That(result.AllTypes.LongValue, Is.EqualTo(expected.AllTypes.LongValue));
            Assert.That(result.AllTypes.GuidValue, Is.EqualTo(expected.AllTypes.GuidValue));
            Assert.That(result.AllTypes.DateTimeValue, Is.EqualTo(expected.AllTypes.DateTimeValue));
            Assert.That(result.AllTypes.Vector2Value, Is.EqualTo(expected.AllTypes.Vector2Value));
            Assert.That(result.AllTypes.Vector3Value, Is.EqualTo(expected.AllTypes.Vector3Value));
            Assert.That(result.AllTypes.Vector4Value, Is.EqualTo(expected.AllTypes.Vector4Value));
            Assert.That(result.AllTypes.QuaternionValue, Is.EqualTo(expected.AllTypes.QuaternionValue));
            Assert.That(result.SimpleDictionary, Is.Not.Null);
            Assert.That(result.SimpleDictionary.Count, Is.EqualTo(2));
            Assert.That(result.SimpleDictionary.Keys, Is.EqualTo(expected.SimpleDictionary.Keys));
            Assert.That(result.SimpleDictionary.Values, Is.EqualTo(expected.SimpleDictionary.Values));
            Assert.That(result.ComplexDictionary, Is.Not.Null);
            Assert.That(result.ComplexDictionary.Count, Is.EqualTo(expected.ComplexDictionary.Count));

            foreach (var keyValuePair in result.ComplexDictionary)
            {
                Assert.That(expected.ComplexDictionary.Keys.Any(x => x.IntValue == keyValuePair.Key.IntValue));
                Assert.That(expected.ComplexDictionary.Values.Any(x => x.FloatValue == keyValuePair.Value.FloatValue));
            }
        }

        [Test]
        public void should_serialize_with_debug_serializer()
        {
            var a = GenerateDummyData();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new DebugSerializer();

            var output = serializer.SerializeData(typeStuff, a);
            Console.WriteLine(output);
        }

        [Test]
        public void should_correctly_serialize_with_json()
        {
            var a = GenerateDummyData();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new JsonSerializer();
            var jsonOutput = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + Encoding.ASCII.GetByteCount(jsonOutput) + " bytes");
            Console.WriteLine(jsonOutput);
            
            var deserializer = new JsonDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, jsonOutput);

            AssertionOnDummyData(a, result);
        }
        
        [Test]
        public void should_correctly_serialize_with_binary()
        {
            var a = GenerateDummyData();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new BinarySerializer();
            var binaryOutput = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + binaryOutput.Length + " bytes");
            Console.WriteLine(BitConverter.ToString(binaryOutput));

            var deserializer = new BinaryDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, binaryOutput);

            AssertionOnDummyData(a, result);
        }

        [Test]
        public void should_correctly_serialize_with_xml()
        {
            var a = GenerateDummyData();
            var typeStuff = _typeMapper.GetTypeMappingsFor(typeof(A));

            var serializer = new XmlSerializer();
            var xmlOutput = serializer.SerializeData(typeStuff, a);
            Console.WriteLine("FileSize: " + Encoding.ASCII.GetByteCount(xmlOutput) + " bytes");
            Console.WriteLine(xmlOutput);

            var deserializer = new XmlDeserializer();
            var result = deserializer.DeserializeData<A>(typeStuff, xmlOutput);

            AssertionOnDummyData(a, result);
        }
    }
}