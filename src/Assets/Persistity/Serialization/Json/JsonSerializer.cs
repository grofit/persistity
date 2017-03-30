using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Exceptions;
using Persistity.Extensions;
using Persistity.Json;
using Persistity.Mappings;
using Persistity.Registries;
using UnityEngine;

namespace Persistity.Serialization.Json
{
    public class JsonSerializer : IJsonSerializer
    {
        public const string TypeField = "Type";
        public const string DataField = "Data";
        public const string KeyField = "Key";
        public const string ValueField = "Value";

        public IMappingRegistry MappingRegistry { get; private set; }
        public JsonConfiguration Configuration { get; private set; }

        public JsonSerializer(IMappingRegistry mappingRegistry, JsonConfiguration configuration = null)
        {
            MappingRegistry = mappingRegistry;
            Configuration = configuration ?? JsonConfiguration.Default;
        }

        private JSONNull GetNullNode()
        { return new JSONNull(); }

        private JSONNode SerializeDefaultPrimitive(object value, Type actualType)
        {
            if (actualType == typeof(byte)) { return new JSONNumber((byte)value); }
            if (actualType == typeof(short)) { return new JSONNumber((short)value); }
            if (actualType == typeof(int)) { return new JSONNumber((int)value); }
            if (actualType == typeof(long)) { return new JSONString(value.ToString()); }
            if (actualType == typeof(Guid)) { return new JSONString(value.ToString()); }
            if (actualType == typeof(bool)) { return new JSONBool((bool)value); }
            if (actualType == typeof(float)) { return new JSONNumber((float)value); }
            if (actualType == typeof(double)) { return new JSONNumber((double)value); }

            if (actualType == typeof(Vector2))
            {
                var typedValue = (Vector2)value;
                var node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                return node;
            }

            if (actualType == typeof(Vector3))
            {
                var typedValue = (Vector3)value;
                var node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                return node;
            }
            if (actualType == typeof(Vector4))
            {
                var typedValue = (Vector4)value;
                var node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                node.Add("w", new JSONNumber(typedValue.w));
                return node;
            }

            if (actualType == typeof(Quaternion))
            {
                var typedValue = (Quaternion)value;
                var node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                node.Add("w", new JSONNumber(typedValue.w));
                return node;
            }

            if (actualType == typeof(DateTime))
            {
                var typedValue = (DateTime)value;
                return new JSONString(typedValue.ToBinary().ToString());
            }

            if (actualType == typeof(string) || actualType.IsEnum)
            { return new JSONString(value.ToString()); }

            throw new Exception("Type is not a default primitive");
        }

        private JSONNode SerializePrimitive(object value, Type type)
        {
            if (value == null) { return GetNullNode(); }

            JSONNode node = null;
            var actualType = type;
            var isDefaultPrimitive = MappingRegistry.TypeMapper.TypeAnalyzer.IsDefaultPrimitiveType(type);
            if (!isDefaultPrimitive)
            {
                var nullableType = Nullable.GetUnderlyingType(type);
            }

            if (actualType == typeof(byte)) { node = new JSONNumber((byte)value); }
            else if (actualType == typeof(short)) { node = new JSONNumber((short)value); }
            else if (actualType == typeof(int)) { node = new JSONNumber((int)value); }
            else if (actualType == typeof(long)) { node = new JSONString(value.ToString()); }
            else if (actualType == typeof(Guid)) { node = new JSONString(value.ToString()); }
            else if (actualType == typeof(bool)) { node = new JSONBool((bool)value); }
            else if (actualType == typeof(float)) { node = new JSONNumber((float)value); }
            else if (actualType == typeof(double)) { node = new JSONNumber((double)value); }
            else if (actualType == typeof(Vector2))
            {
                var typedValue = (Vector2)value;
                node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
            }
            else if (actualType == typeof(Vector3))
            {
                var typedValue = (Vector3)value;
                node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
            }
            else if (actualType == typeof(Vector4))
            {
                var typedValue = (Vector4)value;
                node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                node.Add("w", new JSONNumber(typedValue.w));
            }
            else if (actualType == typeof(Quaternion))
            {
                var typedValue = (Quaternion) value;
                node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                node.Add("w", new JSONNumber(typedValue.w));
            }
            else if (actualType == typeof(DateTime))
            {
                var typedValue = (DateTime) value;
                node = new JSONString(typedValue.ToBinary().ToString());
            }
            else if (actualType == typeof(string) || actualType.IsEnum)
            { node = new JSONString(value.ToString()); }
            else
            {
                node = new JSONObject();
                var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(actualType));
                if(matchingHandler == null) { throw new NoKnownTypeException(actualType); }
                matchingHandler.HandleTypeSerialization(node, value);
            }

            return node;
        }

        public DataObject Serialize(object data)
        {
            var dataType = data.GetType();
            var typeMapping = MappingRegistry.GetMappingFor(dataType);

            var jsonNode = Serialize(typeMapping.InternalMappings, data);
            jsonNode.Add(TypeField, typeMapping.Type.GetPersistableName());

            var jsonString = jsonNode.ToString();
            return new DataObject(jsonString);
        }

        private JSONNode SerializeProperty<T>(PropertyMapping propertyMapping, T data)
        {
            if (data == null) { return GetNullNode(); }
            var underlyingValue = propertyMapping.GetValue(data);
            if (underlyingValue == null) { return GetNullNode(); }
            
            return SerializePrimitive(underlyingValue, propertyMapping.Type);
        }

        private JSONNode SerializeNestedObject<T>(NestedMapping nestedMapping, T data)
        {
            if (data == null) { return GetNullNode(); }
            var currentData = nestedMapping.GetValue(data);
            if (currentData == null) { return GetNullNode(); }

            if (!nestedMapping.IsDynamicType)
            { return Serialize(nestedMapping.InternalMappings, currentData); }
            
            var typeToUse = currentData.GetType();
            var jsonObject = new JSONObject();
            jsonObject.Add(TypeField, typeToUse.GetPersistableName());

            var isPrimitiveType = MappingRegistry.TypeMapper.TypeAnalyzer.IsPrimitiveType(typeToUse);
            if (isPrimitiveType)
            {
                var jsonData = SerializePrimitive(currentData, typeToUse);
                jsonObject.Add(DataField, jsonData);
                return jsonObject;
            }
            else
            {
                var mapping = MappingRegistry.GetMappingFor(typeToUse);
                var jsonData = Serialize(mapping.InternalMappings, currentData);
                jsonObject.Add(DataField, jsonData);
                return jsonObject;
            }
        }
        
        private JSONNode SerializeCollection<T>(CollectionMapping collectionMapping, T data)
        {
            if (data == null) { return GetNullNode(); }
            var collectionValue = collectionMapping.GetValue(data);
            if (collectionValue == null) { return GetNullNode(); }

            var jsonArray = new JSONArray();

            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];

                if (currentData == null)
                {
                    jsonArray.Add(GetNullNode());
                    continue;
                }

                var mappings = collectionMapping.InternalMappings;
                if (!collectionMapping.IsElementDynamicType)
                {
                    if (mappings.Count > 0)
                    {
                        var result = Serialize(mappings, currentData);
                        jsonArray.Add(result);
                    }
                    else
                    {
                        var result = SerializePrimitive(currentData, collectionMapping.CollectionType);
                        jsonArray.Add(result);
                    }
                    continue;
                }

                var typeToUse = currentData.GetType();
                var jsonTypeData = new JSONObject();
                jsonTypeData.Add(TypeField, typeToUse.GetPersistableName());

                if (MappingRegistry.TypeMapper.TypeAnalyzer.IsPrimitiveType(typeToUse))
                {
                    var result = SerializePrimitive(currentData, typeToUse);
                    jsonTypeData.Add(DataField, result);
                }
                else
                {
                    var mapping = MappingRegistry.GetMappingFor(typeToUse);
                    mappings = mapping.InternalMappings;

                    if (mappings.Count > 0)
                    {
                        var result = Serialize(mappings, currentData);
                        jsonTypeData.Add(DataField, result);
                    }
                    else
                    {
                        var result = SerializePrimitive(currentData, typeToUse);
                        jsonTypeData.Add(DataField, result);
                    }
                }

                jsonArray.Add(jsonTypeData);
            }

            return jsonArray;
        }

        private JSONNode SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data)
        {
            if(data == null) { return new JSONArray(); }

            var jsonArray = new JSONArray();
            var dictionaryValue = dictionaryMapping.GetValue(data);
            if (dictionaryValue == null) { return GetNullNode(); }

            foreach (var currentKey in dictionaryValue.Keys)
            {
                JSONNode jsonKey, jsonValue;

                if (!dictionaryMapping.IsKeyDynamicType)
                {
                    if (dictionaryMapping.KeyMappings.Count > 0)
                    { jsonKey = Serialize(dictionaryMapping.KeyMappings, currentKey); }
                    else
                    { jsonKey = SerializePrimitive(currentKey, dictionaryMapping.KeyType); }
                }
                else
                {
                    var typeToUse = currentKey.GetType();
                    var jsonTypeData = new JSONObject();
                    jsonTypeData.Add(TypeField, typeToUse.GetPersistableName());

                    JSONNode dataNode;
                    if (MappingRegistry.TypeMapper.TypeAnalyzer.IsPrimitiveType(typeToUse))
                    { dataNode = SerializePrimitive(currentKey, typeToUse); }
                    else
                    {
                        var typeMapping = MappingRegistry.GetMappingFor(typeToUse);
                        dataNode = Serialize(typeMapping.InternalMappings, currentKey);
                    }

                    jsonTypeData.Add(DataField, dataNode);
                    jsonKey = jsonTypeData;
                }

                var currentValue = dictionaryValue[currentKey];
                if (!dictionaryMapping.IsValueDynamicType)
                {
                    if (currentValue == null)
                    { jsonValue = GetNullNode(); }
                    else if (dictionaryMapping.ValueMappings.Count > 0)
                    { jsonValue = Serialize(dictionaryMapping.ValueMappings, currentValue); }
                    else
                    { jsonValue = SerializePrimitive(currentValue, dictionaryMapping.ValueType); }
                }
                else
                {
                    var typeToUse = currentValue.GetType();
                    var jsonTypeData = new JSONObject();
                    jsonTypeData.Add(TypeField, typeToUse.GetPersistableName());

                    JSONNode dataNode;
                    if (MappingRegistry.TypeMapper.TypeAnalyzer.IsPrimitiveType(typeToUse))
                    { dataNode = SerializePrimitive(currentValue, typeToUse); }
                    else
                    {
                        var typeMapping = MappingRegistry.GetMappingFor(typeToUse);
                        dataNode = Serialize(typeMapping.InternalMappings, currentValue);
                    }

                    jsonTypeData.Add(DataField, dataNode);
                    jsonValue = jsonTypeData;
                }

                var jsonKeyValue = new JSONObject();
                jsonKeyValue.Add(KeyField, jsonKey);
                jsonKeyValue.Add(ValueField, jsonValue);
                jsonArray.Add(jsonKeyValue);
            }

            return jsonArray;
        }

        private JSONNode Serialize<T>(IEnumerable<Mapping> mappings, T data)
        {
            var jsonNode = new JSONObject();

            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                {
                    var result = SerializeProperty((mapping as PropertyMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
                else if (mapping is NestedMapping)
                {
                    var result = SerializeNestedObject((mapping as NestedMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
                else if (mapping is DictionaryMapping)
                {
                    var result = SerializeDictionary((mapping as DictionaryMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
                else
                {
                    var result = SerializeCollection((mapping as CollectionMapping), data);
                    jsonNode.Add(mapping.LocalName, result);
                }
            }
            return jsonNode;
        }
    }
}