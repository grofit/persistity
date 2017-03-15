using System;
using System.Collections;
using System.Collections.Generic;
using Persistity.Json;
using Persistity.Mappings;
using UnityEngine;

namespace Persistity.Serialization
{
    public class JsonDeserializer : IDeserializer<string>
    {
        private object DeserializePrimitive(JSONNode value, Type type)
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
            Deserialize(typeMapping.InternalMappings, jsonData, instance);
            return instance;
        }

        private void DeserializeProperty<T>(PropertyMapping propertyMapping, JSONNode data, T instance)
        {
            var underlyingValue = DeserializePrimitive(data, propertyMapping.Type);
            propertyMapping.SetValue(instance, underlyingValue);
        }

        private void DeserializeNestedObject<T>(NestedMapping nestedMapping, JSONNode data, T instance)
        { Deserialize(nestedMapping.InternalMappings, data, instance); }

        private void DeserializeCollection(CollectionMapping collectionMapping, JSONArray data, IList instance)
        {
            for(var i=0;i<data.Count;i++)
            {
                if (collectionMapping.InternalMappings.Count > 0)
                {
                    var elementInstance = Activator.CreateInstance(collectionMapping.CollectionType);
                    Deserialize(collectionMapping.InternalMappings, data[i], elementInstance);

                    if (instance.IsFixedSize)
                    { instance[i] = elementInstance; }
                    else
                    { instance.Insert(i, elementInstance); }
                }
                else
                {
                    var value = DeserializePrimitive(data[i], collectionMapping.CollectionType);
                    if (instance.IsFixedSize)
                    { instance[i] = value; }
                    else
                    { instance.Insert(i, value); }
                }
            }
        }

        private void DeserializeDictionary(DictionaryMapping dictionaryMapping, JSONArray data, IDictionary instance)
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
                    Deserialize(dictionaryMapping.KeyMappings, jsonKey, currentKey);
                }
                else
                { currentKey = DeserializePrimitive(jsonKey, dictionaryMapping.KeyType); }

                if (dictionaryMapping.ValueMappings.Count > 0)
                {
                    currentValue = Activator.CreateInstance(dictionaryMapping.ValueType);
                    Deserialize(dictionaryMapping.ValueMappings, jsonValue, currentValue);
                }
                else
                { currentValue = DeserializePrimitive(jsonValue, dictionaryMapping.ValueType); }

                instance.Add(currentKey, currentValue);
            }
        }

        private void Deserialize<T>(IEnumerable<Mapping> mappings, JSONNode jsonNode, T instance)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var jsonData = jsonNode[mapping.LocalName];
                    DeserializeProperty((mapping as PropertyMapping), jsonData, instance);                   
                }
                else if (mapping is NestedMapping)
                {
                    var nestedMapping = (mapping as NestedMapping);
                    var jsonData = jsonNode[mapping.LocalName];
                    var childInstance = Activator.CreateInstance(nestedMapping.Type);
                    DeserializeNestedObject(nestedMapping, jsonData, childInstance);
                    nestedMapping.SetValue(instance, childInstance);
                }
                else if (mapping is DictionaryMapping)
                {
                    var dictionaryMapping = (mapping as DictionaryMapping);
                    var jsonData = jsonNode[mapping.LocalName].AsArray;
                    var dictionarytype = typeof(Dictionary<,>);
                    var constructedDictionaryType = dictionarytype.MakeGenericType(dictionaryMapping.KeyType, dictionaryMapping.ValueType);
                    var dictionary = (IDictionary)Activator.CreateInstance(constructedDictionaryType);
                    DeserializeDictionary(dictionaryMapping, jsonData, dictionary);
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
                        DeserializeCollection(collectionMapping, jsonData, arrayInstance);
                        collectionMapping.SetValue(instance, arrayInstance);
                    }
                    else
                    {
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(collectionMapping.CollectionType);
                        var listInstance = (IList)Activator.CreateInstance(constructedListType);
                        DeserializeCollection(collectionMapping, jsonData, listInstance);
                        collectionMapping.SetValue(instance, listInstance);
                    }
                }
            }
        }
    }
}