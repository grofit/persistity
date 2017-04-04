using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Tests.Editor;
using NUnit.Framework;
using Tests.Editor.Models;
using UnityEngine;

namespace Tests.Editor.Helpers
{
    public static class SerializationTestHelper
    {
        public static ComplexModel GeneratePopulatedModel()
        {
            var a = new ComplexModel();
            a.TestValue = "WOW";
            a.NonPersisted = 100;
            a.Stuff.Add("woop");
            a.Stuff.Add("poow");

            a.NestedValue = new B
            {
                IntValue = 0,
                StringValue = "Hello",
                NestedArray = new[] { new C { FloatValue = 2.43f } }
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
                QuaternionValue = new Quaternion(1.0f, 1.0f, 1.0f, 1.0f),
                SomeType = SomeTypes.Known
            };

            a.SimpleDictionary.Add("key1", "some-value");
            a.SimpleDictionary.Add("key2", "some-other-value");

            a.ComplexDictionary.Add(new E { IntValue = 10 }, new C { FloatValue = 32.2f });

            return a;
        }

        public static ComplexModel GenerateNulledModel()
        {
            var a = new ComplexModel();
            a.TestValue = null;
            a.NonPersisted = 0;
            a.Stuff = null;
            a.NestedValue = null;
            a.NestedArray = new B[2];
            a.NestedArray[0] = new B
            {
                IntValue = 0,
                StringValue = null,
                NestedArray = null
            };
            a.NestedArray[1] = null;
            a.AllTypes = null;
            a.SimpleDictionary.Add("key1", null);
            a.ComplexDictionary = null;

            return a;
        }

        public static NullableTypesModel GenerateNulledNullableModel()
        {
            var model = new NullableTypesModel();
            model.NullableFloat = null;
            model.NullableVector3 = null;
            model.NullableInt = null;
            return model;
        }

        public static NullableTypesModel GeneratePopulatedNullableModel()
        {
            var model = new NullableTypesModel();
            model.NullableFloat = 10.0f;
            model.NullableVector3 = Vector3.one;
            model.NullableInt = 22;
            return model;
        }

        public static DynamicTypesModel GeneratePopulatedDynamicTypesModel()
        {
            var model = new DynamicTypesModel();
            model.DynamicNestedProperty = new E { IntValue = 10 };
            model.DynamicPrimitiveProperty = 12;

            model.DynamicList = new List<object>();
            model.DynamicList.Add(new E() { IntValue = 22 });
            model.DynamicList.Add(new C() { FloatValue = 25 });
            model.DynamicList.Add(20);

            model.DynamicDictionary = new Dictionary<object, object>();
            model.DynamicDictionary.Add("key1", 62);
            model.DynamicDictionary.Add(new E{IntValue = 99}, 54);
            model.DynamicDictionary.Add(1, new C {FloatValue = 51.0f});
            return model;
        }

        public static DynamicTypesModel GenerateNulledDynamicTypesModel()
        {
            var model = new DynamicTypesModel();
            model.DynamicNestedProperty = null;
            model.DynamicPrimitiveProperty = null;

            model.DynamicList = new List<object>();
            model.DynamicList.Add(new E() { IntValue = 22 });
            model.DynamicList.Add(null);
            model.DynamicList.Add(20);

            model.DynamicDictionary = new Dictionary<object, object>();
            model.DynamicDictionary.Add("key1", null);
            model.DynamicDictionary.Add(new E { IntValue = 99 }, null);
            model.DynamicDictionary.Add(1, null);
            return model;
        }

        public static void AssertPopulatedData(ComplexModel expected, ComplexModel actual)
        {
            Assert.That(actual.TestValue, Is.EqualTo(expected.TestValue));
            Assert.That(actual.NonPersisted, Is.EqualTo(0));
            Assert.That(actual.NestedValue, Is.Not.Null);
            Assert.That(actual.NestedValue.IntValue, Is.EqualTo(expected.NestedValue.IntValue));
            Assert.That(actual.NestedValue.StringValue, Is.EqualTo(expected.NestedValue.StringValue));
            Assert.That(actual.NestedValue.NestedArray, Is.Not.Null);
            Assert.That(actual.NestedValue.NestedArray.Length, Is.EqualTo(expected.NestedValue.NestedArray.Length));
            Assert.That(actual.NestedValue.NestedArray[0].FloatValue, Is.EqualTo(expected.NestedValue.NestedArray[0].FloatValue));
            Assert.That(actual.NestedArray, Is.Not.Null);
            Assert.That(actual.NestedArray.Length, Is.EqualTo(expected.NestedArray.Length));
            Assert.That(actual.NestedArray[0].IntValue, Is.EqualTo(expected.NestedArray[0].IntValue));
            Assert.That(actual.NestedArray[0].StringValue, Is.EqualTo(expected.NestedArray[0].StringValue));
            Assert.That(actual.NestedArray[0].NestedArray, Is.Not.Null);
            Assert.That(actual.NestedArray[0].NestedArray.Length, Is.EqualTo(expected.NestedArray[0].NestedArray.Length));
            Assert.That(actual.NestedArray[0].NestedArray[0].FloatValue, Is.EqualTo(expected.NestedArray[0].NestedArray[0].FloatValue));
            Assert.That(actual.NestedArray[1].IntValue, Is.EqualTo(expected.NestedArray[1].IntValue));
            Assert.That(actual.NestedArray[1].StringValue, Is.EqualTo(expected.NestedArray[1].StringValue));
            Assert.That(actual.NestedArray[1].NestedArray, Is.Not.Null);
            Assert.That(actual.NestedArray[1].NestedArray.Length, Is.EqualTo(expected.NestedArray[1].NestedArray.Length));
            Assert.That(actual.NestedArray[1].NestedArray[0].FloatValue, Is.EqualTo(expected.NestedArray[1].NestedArray[0].FloatValue));
            Assert.That(actual.NestedArray[1].NestedArray[1].FloatValue, Is.EqualTo(expected.NestedArray[1].NestedArray[1].FloatValue));
            Assert.That(actual.AllTypes, Is.Not.Null);
            Assert.That(actual.AllTypes.ByteValue, Is.EqualTo(expected.AllTypes.ByteValue));
            Assert.That(actual.AllTypes.ShortValue, Is.EqualTo(expected.AllTypes.ShortValue));
            Assert.That(actual.AllTypes.IntValue, Is.EqualTo(expected.AllTypes.IntValue));
            Assert.That(actual.AllTypes.LongValue, Is.EqualTo(expected.AllTypes.LongValue));
            Assert.That(actual.AllTypes.GuidValue, Is.EqualTo(expected.AllTypes.GuidValue));
            Assert.That(actual.AllTypes.DateTimeValue, Is.EqualTo(expected.AllTypes.DateTimeValue));
            Assert.That(actual.AllTypes.Vector2Value, Is.EqualTo(expected.AllTypes.Vector2Value));
            Assert.That(actual.AllTypes.Vector3Value, Is.EqualTo(expected.AllTypes.Vector3Value));
            Assert.That(actual.AllTypes.Vector4Value, Is.EqualTo(expected.AllTypes.Vector4Value));
            Assert.That(actual.AllTypes.QuaternionValue, Is.EqualTo(expected.AllTypes.QuaternionValue));
            Assert.That(actual.AllTypes.SomeType, Is.EqualTo(expected.AllTypes.SomeType));
            Assert.That(actual.SimpleDictionary, Is.Not.Null);
            Assert.That(actual.SimpleDictionary.Count, Is.EqualTo(expected.SimpleDictionary.Count));
            Assert.That(actual.SimpleDictionary.Keys, Is.EqualTo(expected.SimpleDictionary.Keys));
            Assert.That(actual.SimpleDictionary.Values, Is.EqualTo(expected.SimpleDictionary.Values));
            Assert.That(actual.ComplexDictionary, Is.Not.Null);
            Assert.That(actual.ComplexDictionary.Count, Is.EqualTo(expected.ComplexDictionary.Count));

            foreach (var keyValuePair in actual.ComplexDictionary)
            {
                Assert.That(expected.ComplexDictionary.Keys.Any(x => x.IntValue == keyValuePair.Key.IntValue));
                Assert.That(expected.ComplexDictionary.Values.Any(x => x.FloatValue == keyValuePair.Value.FloatValue));
            }
        }

        public static void AssertNulledData(ComplexModel expected, ComplexModel actual)
        {
            Assert.That(actual.TestValue, Is.EqualTo(expected.TestValue));
            Assert.That(actual.NonPersisted, Is.EqualTo(0));
            Assert.That(actual.NestedValue, Is.Null);
            Assert.That(actual.NestedArray, Is.Not.Null);
            Assert.That(actual.NestedArray.Length, Is.EqualTo(expected.NestedArray.Length));
            Assert.That(actual.NestedArray[0].IntValue, Is.EqualTo(expected.NestedArray[0].IntValue));
            Assert.That(actual.NestedArray[0].StringValue, Is.EqualTo(expected.NestedArray[0].StringValue));
            Assert.That(actual.NestedArray[0].NestedArray, Is.Null);
            Assert.That(actual.NestedArray[1], Is.Null);
            Assert.That(actual.AllTypes, Is.Null);
            Assert.That(actual.SimpleDictionary, Is.Not.Null);
            Assert.That(actual.SimpleDictionary.Count, Is.EqualTo(expected.SimpleDictionary.Count));
            Assert.That(actual.SimpleDictionary.Keys, Is.EqualTo(expected.SimpleDictionary.Keys));
            Assert.That(actual.SimpleDictionary.Values, Is.EqualTo(expected.SimpleDictionary.Values));
            Assert.That(actual.ComplexDictionary, Is.Null);
        }

        public static void AssertPopulatedDynamicTypesData(DynamicTypesModel expected, DynamicTypesModel actual)
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.DynamicNestedProperty, Is.TypeOf(expected.DynamicNestedProperty.GetType()));

            var typedNestedProperty = (E)actual.DynamicNestedProperty;
            Assert.That(typedNestedProperty, Is.Not.Null);
            Assert.That(typedNestedProperty.IntValue, Is.EqualTo(((E)expected.DynamicNestedProperty).IntValue));

            var typedPrimitiveProperty = (int) actual.DynamicPrimitiveProperty;
            Assert.That(typedPrimitiveProperty, Is.EqualTo((int) expected.DynamicPrimitiveProperty));

            Assert.That(actual.DynamicList.Count, Is.Not.Null);
            Assert.That(actual.DynamicList.Count, Is.EqualTo(expected.DynamicList.Count));
            Assert.That(actual.DynamicList[0], Is.TypeOf(expected.DynamicList[0].GetType()));
            Assert.That((actual.DynamicList[0] as E).IntValue, Is.EqualTo((expected.DynamicList[0] as E).IntValue));
            Assert.That(actual.DynamicList[1], Is.TypeOf(expected.DynamicList[1].GetType()));
            Assert.That((actual.DynamicList[1] as C).FloatValue, Is.EqualTo((expected.DynamicList[1] as C).FloatValue));
            Assert.That(actual.DynamicList[2], Is.TypeOf(expected.DynamicList[2].GetType()));
            Assert.That(actual.DynamicList[2], Is.EqualTo(expected.DynamicList[2]));

            Assert.That(actual.DynamicDictionary, Is.Not.Null);
            Assert.That(actual.DynamicDictionary.Count, Is.EqualTo(expected.DynamicDictionary.Count));

            var expectedFirstKey = expected.DynamicDictionary.Keys.ElementAt(0);
            var actualFirstKey = actual.DynamicDictionary.Keys.ElementAt(0);
            Assert.That(actualFirstKey, Is.EqualTo(expectedFirstKey));
            Assert.That(actual.DynamicDictionary[actualFirstKey], Is.EqualTo(expected.DynamicDictionary[expectedFirstKey]));

            var expectedSecondKey = (E)expected.DynamicDictionary.Keys.ElementAt(1);
            var actualSecondKey = (E)actual.DynamicDictionary.Keys.ElementAt(1);
            Assert.That(actualSecondKey.IntValue, Is.EqualTo(expectedSecondKey.IntValue));
            Assert.That(actual.DynamicDictionary[actualSecondKey], Is.EqualTo(expected.DynamicDictionary[expectedSecondKey]));

            var expectedThirdKey = expected.DynamicDictionary.Keys.ElementAt(2);
            var actualThirdKey = actual.DynamicDictionary.Keys.ElementAt(2);
            Assert.That(actualThirdKey, Is.EqualTo(expectedThirdKey));
            Assert.That((actual.DynamicDictionary[actualThirdKey] as C).FloatValue, Is.EqualTo((expected.DynamicDictionary[expectedThirdKey] as C).FloatValue));
        }

        public static void AsserNulledDynamicTypesData(DynamicTypesModel expected, DynamicTypesModel actual)
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.DynamicNestedProperty, Is.EqualTo(expected.DynamicNestedProperty));
            Assert.That(actual.DynamicPrimitiveProperty, Is.EqualTo(expected.DynamicPrimitiveProperty));

            Assert.That(actual.DynamicList, Is.Not.Null);
            Assert.That(actual.DynamicList.Count, Is.EqualTo(expected.DynamicList.Count));
            Assert.That(actual.DynamicList[0], Is.TypeOf(expected.DynamicList[0].GetType()));
            Assert.That((actual.DynamicList[0] as E).IntValue, Is.EqualTo((expected.DynamicList[0] as E).IntValue));
            Assert.That(actual.DynamicList[1], Is.EqualTo(expected.DynamicList[1]));
            Assert.That(actual.DynamicList[2], Is.EqualTo(expected.DynamicList[2]));

            Assert.That(actual.DynamicDictionary, Is.Not.Null);
            Assert.That(actual.DynamicDictionary.Count, Is.EqualTo(expected.DynamicDictionary.Count));

            var expectedFirstKey = expected.DynamicDictionary.Keys.ElementAt(0);
            var actualFirstKey = actual.DynamicDictionary.Keys.ElementAt(0);
            Assert.That(actualFirstKey, Is.EqualTo(expectedFirstKey));
            Assert.That(actual.DynamicDictionary[actualFirstKey], Is.EqualTo(expected.DynamicDictionary[expectedFirstKey]));

            var expectedSecondKey = (E)expected.DynamicDictionary.Keys.ElementAt(1);
            var actualSecondKey = (E)actual.DynamicDictionary.Keys.ElementAt(1);
            Assert.That(actualSecondKey.IntValue, Is.EqualTo(expectedSecondKey.IntValue));
            Assert.That(actual.DynamicDictionary[actualSecondKey], Is.EqualTo(expected.DynamicDictionary[expectedSecondKey]));

            var expectedThirdKey = expected.DynamicDictionary.Keys.ElementAt(2);
            var actualThirdKey = actual.DynamicDictionary.Keys.ElementAt(2);
            Assert.That(actualThirdKey, Is.EqualTo(expectedThirdKey));
            Assert.That(actual.DynamicDictionary[actualThirdKey], Is.EqualTo(expected.DynamicDictionary[expectedThirdKey]));
        }

        public static void AssertNullableModelData(NullableTypesModel expected, NullableTypesModel actual)
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.NullableFloat, Is.EqualTo(expected.NullableFloat));
            Assert.That(actual.NullableInt, Is.EqualTo(expected.NullableInt));
            Assert.That(actual.NullableVector3, Is.EqualTo(expected.NullableVector3));
        }
    }
}