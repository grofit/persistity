using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Persistity.Json;
using Persistity.Mappings;
using Persistity.Registries;
using UnityEngine;

namespace Persistity.Serialization.Json
{
    public class JsonDeserializer : IJsonDeserializer
    {
        public IMappingRegistry MappingRegistry { get; private set; }
        public JsonConfiguration Configuration { get; private set; }

        public JsonDeserializer(IMappingRegistry mappingRegistry, JsonConfiguration configuration = null)
        {
            MappingRegistry = mappingRegistry;
            Configuration = configuration ?? JsonConfiguration.Default;
        }

        private bool IsNullNode(JSONNode node)
        { return node == null; }

        private object DeserializePrimitive(Type type, JSONNode value)
        {
            if (type == typeof(byte)) { return (byte)value.AsInt; }
            if (type == typeof(short)) { return (short)value.AsInt; }
            if (type == typeof(int)) { return value.AsInt; }
            if (type == typeof(long)) { return long.Parse(value.Value); }
            if (type == typeof(Guid)) { return new Guid(value.Value); }
            if (type == typeof(bool)) { return value.AsBool; }
            if (type == typeof(float)) { return value.AsFloat; }
            if (type == typeof(double)) { return value.AsDouble; }
            if (type.IsEnum) { return Enum.Parse(type, value.Value); }
            if (type == typeof(DateTime)) { return DateTime.FromBinary(long.Parse(value.Value)); }
            if (type.IsEnum) { return Enum.Parse(type, value.Value); }
            if (type == typeof(Vector2))
            { return new Vector2(value["x"].AsFloat, value["y"].AsFloat); }
            if (type == typeof(Vector3))
            { return new Vector3(value["x"].AsFloat, value["y"].AsFloat, value["z"].AsFloat); }
            if (type == typeof(Vector4))
            { return new Vector4(value["x"].AsFloat, value["y"].AsFloat, value["z"].AsFloat, value["w"].AsFloat); }
            if (type == typeof(Quaternion))
            { return new Quaternion(value["x"].AsFloat, value["y"].AsFloat, value["z"].AsFloat, value["w"].AsFloat); }

            var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
            if (matchingHandler != null)
            { return matchingHandler.HandleTypeDeserialization(value); }

            return value.Value;
        }

        public T Deserialize<T>(DataObject data) where T : new()
        { return (T)Deserialize(data); }

        public object Deserialize(DataObject data)
        {
            var jsonData = JSON.Parse(data.AsString);
            var typeName = jsonData[JsonSerializer.TypeField].Value;
            var type = MappingRegistry.TypeMapper.LoadType(typeName);
            var typeMapping = MappingRegistry.GetMappingFor(type);
            var instance = Activator.CreateInstance(type);
            
            Deserialize(typeMapping.InternalMappings, instance, jsonData);
            return instance;
        }

        private void DeserializeCollection(CollectionMapping collectionMapping, IList instance, JSONArray data)
        {
            for(var i=0;i<data.Count;i++)
            {
                var currentElementNode = data[i];

                var typeToUse = collectionMapping.CollectionType;
                var mappings = collectionMapping.InternalMappings;
                object deserializedValue;

                if (collectionMapping.IsElementDynamicType)
                {
                    var jsonType = currentElementNode[JsonSerializer.TypeField];
                    typeToUse = MappingRegistry.TypeMapper.LoadType(jsonType.Value);
                    currentElementNode = currentElementNode[JsonSerializer.DataField];

                    if (MappingRegistry.TypeMapper.IsPrimitiveType(typeToUse))
                    { mappings = new List<Mapping>(); }
                    else
                    {
                        var typeMapping = MappingRegistry.GetMappingFor(typeToUse);
                        mappings = typeMapping.InternalMappings;
                    }
                }

                if(IsNullNode(currentElementNode))
                { deserializedValue = null; }
                else if (mappings.Count > 0)
                {
                    deserializedValue = Activator.CreateInstance(typeToUse);
                    Deserialize(mappings, deserializedValue, currentElementNode);
                }
                else
                { deserializedValue = DeserializePrimitive(typeToUse, currentElementNode); }
                
                if (instance.IsFixedSize)
                { instance[i] = deserializedValue; }
                else
                { instance.Insert(i, deserializedValue); }
            }
        }

        private void DeserializeDictionary(DictionaryMapping dictionaryMapping, IDictionary instance, JSONArray data)
        {
            for (var i = 0; i < data.Count; i++)
            {
                var currentElement = data[i];
                var jsonKey = currentElement[JsonSerializer.KeyField];
                var jsonValue = currentElement[JsonSerializer.ValueField];
                
                object currentKey, currentValue;

                if (dictionaryMapping.KeyMappings.Count > 0)
                {
                    currentKey = Activator.CreateInstance(dictionaryMapping.KeyType);
                    Deserialize(dictionaryMapping.KeyMappings, currentKey, jsonKey);
                }
                else
                { currentKey = DeserializePrimitive(dictionaryMapping.KeyType, jsonKey); }

                if (IsNullNode(jsonValue))
                { currentValue = null; }
                else if (dictionaryMapping.ValueMappings.Count > 0)
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
                { HandlePropertyMapping(mapping as PropertyMapping, instance, jsonNode); }
                else if (mapping is NestedMapping)
                { HandleNestedMapping(mapping as NestedMapping, instance, jsonNode); }
                else if (mapping is DictionaryMapping)
                { HandleDictionaryMapping(mapping as DictionaryMapping, instance, jsonNode); }
                else
                { HandleCollectionMapping(mapping as CollectionMapping, instance, jsonNode); }
            }
        }

        private void HandlePropertyMapping<T>(PropertyMapping mapping, T instance, JSONNode jsonNode)
        {
            var jsonData = jsonNode[mapping.LocalName];
            if (IsNullNode(jsonData))
            {
                mapping.SetValue(instance, null);
                return;
            }

            var underlyingValue = DeserializePrimitive(mapping.Type, jsonData);
            mapping.SetValue(instance, underlyingValue);
        }

        private void HandleNestedMapping<T>(NestedMapping mapping, T instance, JSONNode jsonNode)
        {
            var jsonData = jsonNode[mapping.LocalName];

            if (IsNullNode(jsonData))
            {
                mapping.SetValue(instance, null);
                return;
            }

            if (!mapping.IsDynamicType)
            {
                var childInstance = Activator.CreateInstance(mapping.Type);
                Deserialize(mapping.InternalMappings, childInstance, jsonData);
                mapping.SetValue(instance, childInstance);
                return;
            }
            
            var jsonDynamicType = jsonData[JsonSerializer.TypeField];
            var jsonDynamicData = jsonData[JsonSerializer.DataField];
            var instanceType = MappingRegistry.TypeMapper.LoadType(jsonDynamicType.Value);
            if (MappingRegistry.TypeMapper.IsPrimitiveType(instanceType))
            {
                var primitiveValue = DeserializePrimitive(instanceType, jsonDynamicData);
                mapping.SetValue(instance, primitiveValue);
                return;
            }

            var typeMapping = MappingRegistry.GetMappingFor(instanceType);
            var dynamicChildInstance = Activator.CreateInstance(instanceType);
            Deserialize(typeMapping.InternalMappings, dynamicChildInstance, jsonDynamicData);
            mapping.SetValue(instance, dynamicChildInstance);
        }

        private void HandleCollectionMapping<T>(CollectionMapping mapping, T instance, JSONNode jsonNode)
        {
            var jsonData = jsonNode[mapping.LocalName].AsArray;

            if (IsNullNode(jsonData))
            {
                mapping.SetValue(instance, null);
                return;
            }

            var arrayCount = jsonData.Count;
            IList collectionInstance;

            if (mapping.IsArray)
            { collectionInstance = (IList) Activator.CreateInstance(mapping.Type, arrayCount); }
            else
            {
                var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(mapping.CollectionType);
                collectionInstance = (IList) Activator.CreateInstance(constructedListType);
            }

            DeserializeCollection(mapping, collectionInstance, jsonData);
            mapping.SetValue(instance, collectionInstance);
        }

        private void HandleDictionaryMapping<T>(DictionaryMapping mapping, T instance, JSONNode jsonNode)
        {
            var jsonData = jsonNode[mapping.LocalName].AsArray;
            if (IsNullNode(jsonData))
            {
                mapping.SetValue(instance, null);
            }
            else
            {
                var dictionarytype = typeof(Dictionary<,>);
                var constructedDictionaryType = dictionarytype.MakeGenericType(mapping.KeyType, mapping.ValueType);
                var dictionary = (IDictionary) Activator.CreateInstance(constructedDictionaryType);
                DeserializeDictionary(mapping, dictionary, jsonData);
                mapping.SetValue(instance, dictionary);
            }
        }
    }
}