using System;
using System.Collections;
using System.Collections.Generic;
using Persistity.Json;
using Persistity.Mappings;
using UnityEngine;

namespace Persistity.Serialization.Json
{
    public class JsonDeserializer : IJsonDeserializer
    {
        private object DeserializePrimitive(Type type, JSONNode value)
        {
            if (type == typeof(byte)) return (byte)value.AsInt;
            if (type == typeof(short)) return (short)value.AsInt;
            if (type == typeof(int)) return value.AsInt;
            if (type == typeof(long)) return long.Parse(value.Value);
            if (type == typeof(Guid)) return new Guid(value.Value);
            if (type == typeof(bool)) return value.AsBool;
            if (type == typeof(float)) return value.AsFloat;
            if (type == typeof(double)) return value.AsDouble;
            if (type == typeof(Vector2)) return value.AsVector2;
            if (type == typeof(Vector3)) return value.AsVector3;
            if (type == typeof(Vector4)) return value.AsVector4;
            if (type == typeof(DateTime)) return DateTime.FromBinary(long.Parse(value.Value));
            if (type == typeof(Quaternion))
            {
                return new Quaternion(value["x"].AsFloat, value["y"].AsFloat, value["z"].AsFloat, value["w"].AsFloat);
            }
            return value.Value;
        }

        public T DeserializeData<T>(TypeMapping typeMapping, string data) where T : new()
        {
            var instance = new T();
            var jsonData = JSON.Parse(data);
            Deserialize(typeMapping.InternalMappings, instance, jsonData);
            return instance;
        }

        private void DeserializeProperty<T>(PropertyMapping propertyMapping, T instance, JSONNode data)
        {
            var underlyingValue = DeserializePrimitive(propertyMapping.Type, data);
            propertyMapping.SetValue(instance, underlyingValue);
        }

        private void DeserializeNestedObject<T>(NestedMapping nestedMapping, T instance, JSONNode data)
        { Deserialize(nestedMapping.InternalMappings, instance, data); }

        private void DeserializeCollection(CollectionMapping collectionMapping, IList instance, JSONArray data)
        {
            for(var i=0;i<data.Count;i++)
            {
                if (collectionMapping.InternalMappings.Count > 0)
                {
                    var elementInstance = Activator.CreateInstance(collectionMapping.CollectionType);
                    Deserialize(collectionMapping.InternalMappings, elementInstance, data[i]);

                    if (instance.IsFixedSize)
                    { instance[i] = elementInstance; }
                    else
                    { instance.Insert(i, elementInstance); }
                }
                else
                {
                    var value = DeserializePrimitive(collectionMapping.CollectionType, data[i]);
                    if (instance.IsFixedSize)
                    { instance[i] = value; }
                    else
                    { instance.Insert(i, value); }
                }
            }
        }

        private void DeserializeDictionary(DictionaryMapping dictionaryMapping, IDictionary instance, JSONArray data)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var currentElement = data[i];
                var jsonKey = currentElement["key"];
                var jsonValue = currentElement["value"];
                object currentKey, currentValue;

                if (dictionaryMapping.KeyMappings.Count > 0)
                {
                    currentKey = Activator.CreateInstance(dictionaryMapping.KeyType);
                    Deserialize(dictionaryMapping.KeyMappings, currentKey, jsonKey);
                }
                else
                { currentKey = DeserializePrimitive(dictionaryMapping.KeyType, jsonKey); }

                if (dictionaryMapping.ValueMappings.Count > 0)
                {
                    currentValue = Activator.CreateInstance(dictionaryMapping.ValueType);
                    Deserialize(dictionaryMapping.ValueMappings, currentValue, jsonValue);
                }
                else
                { currentValue = DeserializePrimitive(dictionaryMapping.ValueType, jsonValue); }

                instance.Add(currentKey, currentValue);
            }
        }

        private void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, JSONNode jsonNode)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var jsonData = jsonNode[mapping.LocalName];
                    DeserializeProperty((mapping as PropertyMapping), instance, jsonData);                   
                }
                else if (mapping is NestedMapping)
                {
                    var nestedMapping = (mapping as NestedMapping);
                    var jsonData = jsonNode[mapping.LocalName];
                    var childInstance = Activator.CreateInstance(nestedMapping.Type);
                    DeserializeNestedObject(nestedMapping, childInstance, jsonData);
                    nestedMapping.SetValue(instance, childInstance);
                }
                else if (mapping is DictionaryMapping)
                {
                    var dictionaryMapping = (mapping as DictionaryMapping);
                    var jsonData = jsonNode[mapping.LocalName].AsArray;
                    var dictionarytype = typeof(Dictionary<,>);
                    var constructedDictionaryType = dictionarytype.MakeGenericType(dictionaryMapping.KeyType, dictionaryMapping.ValueType);
                    var dictionary = (IDictionary)Activator.CreateInstance(constructedDictionaryType);
                    DeserializeDictionary(dictionaryMapping, dictionary, jsonData);
                    dictionaryMapping.SetValue(instance, dictionary);
                }
                else
                {
                    var collectionMapping = (mapping as CollectionMapping);
                    var jsonData = jsonNode[mapping.LocalName].AsArray;
                    var arrayCount = jsonData.Count;

                    if (collectionMapping.IsArray)
                    {
                        var arrayInstance = (IList) Activator.CreateInstance(collectionMapping.Type, arrayCount);
                        DeserializeCollection(collectionMapping, arrayInstance, jsonData);
                        collectionMapping.SetValue(instance, arrayInstance);
                    }
                    else
                    {
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(collectionMapping.CollectionType);
                        var listInstance = (IList)Activator.CreateInstance(constructedListType);
                        DeserializeCollection(collectionMapping, listInstance, jsonData);
                        collectionMapping.SetValue(instance, listInstance);
                    }
                }
            }
        }
    }
}