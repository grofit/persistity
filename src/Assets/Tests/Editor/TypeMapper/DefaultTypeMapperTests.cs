using System;
using System.Collections.Generic;
using Assets.Tests.Editor;
using NUnit.Framework;
using Persistity.Extensions;
using Persistity.Mappings;
using Persistity.Mappings.Mappers;
using Persistity.Mappings.Types;
using Tests.Editor.Models;
using UnityEngine;

namespace Tests.Editor.TypeMapper
{
    [TestFixture]
    public class DefaultTypeMapperTests
    {
        private void AssertBModel(IList<Mapping> mappings)
        {
            var stringValueMapping = mappings[0] as PropertyMapping;
            Assert.That(stringValueMapping, Is.Not.Null);
            Assert.That(stringValueMapping.Type, Is.EqualTo(typeof(string)));
            Assert.That(stringValueMapping.LocalName, Is.EqualTo("StringValue"));

            var intValueMapping = mappings[1] as PropertyMapping;
            Assert.That(intValueMapping, Is.Not.Null);
            Assert.That(intValueMapping.Type, Is.EqualTo(typeof(int)));
            Assert.That(intValueMapping.LocalName, Is.EqualTo("IntValue"));

            var nestedArrayMapping = mappings[2] as CollectionMapping;
            Assert.That(nestedArrayMapping, Is.Not.Null);
            Assert.That(nestedArrayMapping.Type, Is.EqualTo(typeof(C[])));
            Assert.That(nestedArrayMapping.LocalName, Is.EqualTo("NestedArray"));

            AssertCModel(nestedArrayMapping.InternalMappings);
        }

        private void AssertCModel(IList<Mapping> mappings)
        {
            var floatValueMapping = mappings[0] as PropertyMapping;
            Assert.That(floatValueMapping, Is.Not.Null);
            Assert.That(floatValueMapping.Type, Is.EqualTo(typeof(float)));
            Assert.That(floatValueMapping.LocalName, Is.EqualTo("FloatValue"));
        }

        private void AssertDModel(IList<Mapping> mappings)
        {
            var byteValueMapping = mappings[0] as PropertyMapping;
            Assert.That(byteValueMapping, Is.Not.Null);
            Assert.That(byteValueMapping.Type, Is.EqualTo(typeof(byte)));
            Assert.That(byteValueMapping.LocalName, Is.EqualTo("ByteValue"));

            var shortValueMapping = mappings[1] as PropertyMapping;
            Assert.That(shortValueMapping, Is.Not.Null);
            Assert.That(shortValueMapping.Type, Is.EqualTo(typeof(short)));
            Assert.That(shortValueMapping.LocalName, Is.EqualTo("ShortValue"));

            var intValueMapping = mappings[2] as PropertyMapping;
            Assert.That(intValueMapping, Is.Not.Null);
            Assert.That(intValueMapping.Type, Is.EqualTo(typeof(int)));
            Assert.That(intValueMapping.LocalName, Is.EqualTo("IntValue"));

            var longValueMapping = mappings[3] as PropertyMapping;
            Assert.That(longValueMapping, Is.Not.Null);
            Assert.That(longValueMapping.Type, Is.EqualTo(typeof(long)));
            Assert.That(longValueMapping.LocalName, Is.EqualTo("LongValue"));

            var guidValueMapping = mappings[4] as PropertyMapping;
            Assert.That(guidValueMapping, Is.Not.Null);
            Assert.That(guidValueMapping.Type, Is.EqualTo(typeof(Guid)));
            Assert.That(guidValueMapping.LocalName, Is.EqualTo("GuidValue"));

            var dateTimeValueMapping = mappings[5] as PropertyMapping;
            Assert.That(dateTimeValueMapping, Is.Not.Null);
            Assert.That(dateTimeValueMapping.Type, Is.EqualTo(typeof(DateTime)));
            Assert.That(dateTimeValueMapping.LocalName, Is.EqualTo("DateTimeValue"));

            var vector2ValueMapping = mappings[6] as PropertyMapping;
            Assert.That(vector2ValueMapping, Is.Not.Null);
            Assert.That(vector2ValueMapping.Type, Is.EqualTo(typeof(Vector2)));
            Assert.That(vector2ValueMapping.LocalName, Is.EqualTo("Vector2Value"));

            var vector3ValueMapping = mappings[7] as PropertyMapping;
            Assert.That(vector3ValueMapping, Is.Not.Null);
            Assert.That(vector3ValueMapping.Type, Is.EqualTo(typeof(Vector3)));
            Assert.That(vector3ValueMapping.LocalName, Is.EqualTo("Vector3Value"));

            var vector4ValueMapping = mappings[8] as PropertyMapping;
            Assert.That(vector4ValueMapping, Is.Not.Null);
            Assert.That(vector4ValueMapping.Type, Is.EqualTo(typeof(Vector4)));
            Assert.That(vector4ValueMapping.LocalName, Is.EqualTo("Vector4Value"));

            var quaternionValueMapping = mappings[9] as PropertyMapping;
            Assert.That(quaternionValueMapping, Is.Not.Null);
            Assert.That(quaternionValueMapping.Type, Is.EqualTo(typeof(Quaternion)));
            Assert.That(quaternionValueMapping.LocalName, Is.EqualTo("QuaternionValue"));

            var someTypeMapping = mappings[10] as PropertyMapping;
            Assert.That(someTypeMapping, Is.Not.Null);
            Assert.That(someTypeMapping.Type, Is.EqualTo(typeof(SomeTypes)));
            Assert.That(someTypeMapping.LocalName, Is.EqualTo("SomeType"));
        }

        private void AssertEModel(IList<Mapping> mappings)
        {
            var intValueMapping = mappings[0] as PropertyMapping;
            Assert.That(intValueMapping, Is.Not.Null);
            Assert.That(intValueMapping.Type, Is.EqualTo(typeof(int)));
            Assert.That(intValueMapping.LocalName, Is.EqualTo("IntValue"));
        }

        [Test]
        public void should_correctly_map_type()
        {
            var type = typeof(ComplexModel);
            var typeAnalyzer = new TypeAnalyzer();
            var typeMapper = new DefaultTypeMapper(typeAnalyzer);

            var typeMapping = typeMapper.GetTypeMappingsFor(type);

            Assert.That(typeMapping, Is.Not.Null);
            Assert.That(typeMapping.Type, Is.EqualTo(type));
            Assert.That(typeMapping.Name, Is.EqualTo(type.GetPersistableName()));
            Assert.That(typeMapping.InternalMappings.Count, Is.EqualTo(7));

            var testValueMapping = typeMapping.InternalMappings[0] as PropertyMapping;
            Assert.That(testValueMapping.Type, Is.EqualTo(typeof(string)));
            Assert.That(testValueMapping.LocalName, Is.EqualTo("TestValue"));
            Assert.That(testValueMapping.MetaData, Is.Empty);

            var nestedValueMapping = typeMapping.InternalMappings[1] as NestedMapping;
            Assert.That(nestedValueMapping.Type, Is.EqualTo(typeof(B)));
            Assert.That(nestedValueMapping.LocalName, Is.EqualTo("NestedValue"));
            Assert.That(nestedValueMapping.IsDynamicType, Is.False);
            Assert.That(nestedValueMapping.MetaData, Is.Empty);
            AssertBModel(nestedValueMapping.InternalMappings);

            var nestedArrayMapping = typeMapping.InternalMappings[2] as CollectionMapping;
            Assert.That(nestedArrayMapping.Type, Is.EqualTo(typeof(B[])));
            Assert.That(nestedArrayMapping.LocalName, Is.EqualTo("NestedArray"));
            Assert.That(nestedArrayMapping.IsElementDynamicType, Is.False);
            Assert.That(nestedArrayMapping.MetaData, Is.Empty);
            AssertBModel(nestedArrayMapping.InternalMappings);

            var stuffMapping = typeMapping.InternalMappings[3] as CollectionMapping;
            Assert.That(stuffMapping.Type, Is.EqualTo(typeof(IList<string>)));
            Assert.That(stuffMapping.LocalName, Is.EqualTo("Stuff"));
            Assert.That(stuffMapping.IsElementDynamicType, Is.False);
            Assert.That(stuffMapping.MetaData, Is.Empty);
            Assert.That(stuffMapping.CollectionType, Is.EqualTo(typeof(string)));
            Assert.That(stuffMapping.InternalMappings.Count, Is.EqualTo(0));

            var allTypesMapping = typeMapping.InternalMappings[4] as NestedMapping;
            Assert.That(allTypesMapping.Type, Is.EqualTo(typeof(D)));
            Assert.That(allTypesMapping.LocalName, Is.EqualTo("AllTypes"));
            Assert.That(allTypesMapping.IsDynamicType, Is.False);
            Assert.That(allTypesMapping.MetaData, Is.Empty);
            AssertDModel(allTypesMapping.InternalMappings);

            var simpleDictionaryMapping = typeMapping.InternalMappings[5] as DictionaryMapping;
            Assert.That(simpleDictionaryMapping.Type, Is.EqualTo(typeof(IDictionary<string,string>)));
            Assert.That(simpleDictionaryMapping.LocalName, Is.EqualTo("SimpleDictionary"));
            Assert.That(simpleDictionaryMapping.IsKeyDynamicType, Is.False);
            Assert.That(simpleDictionaryMapping.IsValueDynamicType, Is.False);
            Assert.That(simpleDictionaryMapping.MetaData, Is.Empty);
            Assert.That(simpleDictionaryMapping.KeyType, Is.EqualTo(typeof(string)));
            Assert.That(simpleDictionaryMapping.ValueType, Is.EqualTo(typeof(string)));
            Assert.That(simpleDictionaryMapping.KeyMappings.Count, Is.EqualTo(0));
            Assert.That(simpleDictionaryMapping.ValueMappings.Count, Is.EqualTo(0));

            var complexDictionaryMapping = typeMapping.InternalMappings[6] as DictionaryMapping;
            Assert.That(complexDictionaryMapping.Type, Is.EqualTo(typeof(IDictionary<E,C>)));
            Assert.That(complexDictionaryMapping.LocalName, Is.EqualTo("ComplexDictionary"));
            Assert.That(complexDictionaryMapping.IsKeyDynamicType, Is.False);
            Assert.That(complexDictionaryMapping.IsValueDynamicType, Is.False);
            Assert.That(complexDictionaryMapping.MetaData, Is.Empty);
            Assert.That(complexDictionaryMapping.KeyType, Is.EqualTo(typeof(E)));
            Assert.That(complexDictionaryMapping.ValueType, Is.EqualTo(typeof(C)));
            Assert.That(complexDictionaryMapping.KeyMappings.Count, Is.EqualTo(1));
            Assert.That(complexDictionaryMapping.ValueMappings.Count, Is.EqualTo(1));
            AssertEModel(complexDictionaryMapping.KeyMappings);
            AssertCModel(complexDictionaryMapping.ValueMappings);

        }
    }
}