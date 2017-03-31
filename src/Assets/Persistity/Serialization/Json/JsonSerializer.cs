using System;
using System.Collections.Generic;
using Persistity.Extensions;
using Persistity.Json;
using Persistity.Mappings;
using Persistity.Registries;
using Persistity.Serialization.Binary;
using UnityEngine;

namespace Persistity.Serialization.Json
{
    /*
    public class JsonSerializer : GenericSerializer<JSONLazyNode, JSONNode>, IJsonSerializer
    {
        public const string TypeField = "Type";
        public const string DataField = "Data";
        public const string KeyField = "Key";
        public const string ValueField = "Value";
        
        public JsonSerializer(IMappingRegistry mappingRegistry, JsonConfiguration configuration = null) : base(mappingRegistry)
        {
            Configuration = configuration ?? JsonConfiguration.Default;
        }

        protected override void HandleNullData(JSONLazyNode state)
        { state.SetValue(new JSONNull()); }

        protected override void HandleNullObject(JSONLazyNode state)
        { HandleNullData(state); }
        
        protected override void AddCountToState(JSONLazyNode state, int count)
        {}

        protected override void SerializeDefaultPrimitive(object value, Type type, JSONLazyNode state)
        {
            JSONNode outputNode;

            if(value == null) { outputNode = new JSONNull(); }
            else if (type == typeof(byte)) { outputNode = new JSONNumber((byte)value); }
            else if (type == typeof(short)) { outputNode = new JSONNumber((short)value); }
            else if (type == typeof(int)) { outputNode = new JSONNumber((int)value); }
            else if (type == typeof(long)) { outputNode = new JSONString(value.ToString()); }
            else if (type == typeof(Guid)) { outputNode = new JSONString(value.ToString()); }
            else if (type == typeof(bool)) { outputNode = new JSONBool((bool)value); }
            else if (type == typeof(float)) { outputNode = new JSONNumber((float)value); }
            else if (type == typeof(double)) { outputNode = new JSONNumber((double)value); }
            else if (type == typeof(Vector2))
            {
                var typedValue = (Vector2)value;
                var node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                outputNode = node;
            }
            else if (type == typeof(Vector3))
            {
                var typedValue = (Vector3)value;
                var node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                outputNode = node;
            }
            else if (type == typeof(Vector4))
            {
                var typedValue = (Vector4)value;
                var node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                node.Add("w", new JSONNumber(typedValue.w));
                outputNode = node;
            }
            else if (type == typeof(Quaternion))
            {
                var typedValue = (Quaternion)value;
                var node = new JSONObject();
                node.Add("x", new JSONNumber(typedValue.x));
                node.Add("y", new JSONNumber(typedValue.y));
                node.Add("z", new JSONNumber(typedValue.z));
                node.Add("w", new JSONNumber(typedValue.w));
                outputNode = node;
            }
            else if (type == typeof(DateTime))
            {
                var typedValue = (DateTime) value;
                outputNode = new JSONString(typedValue.ToBinary().ToString());
            }
            else if (type == typeof(string) || type.IsEnum)
            { outputNode = new JSONString(value.ToString()); }
            else
            { outputNode = new JSONNull(); }

            state.SetValue(outputNode);
        }

        public override DataObject Serialize(object data)
        {
            var dataType = data.GetType();
            var typeMapping = MappingRegistry.GetMappingFor(dataType);

            var jsonObject = new JSONLazyNode();
            jsonObject.Add(TypeField, typeMapping.Type.GetPersistableName());
            Serialize(typeMapping.InternalMappings, data, jsonObject);
            var jsonString = jsonObject.AsObject.ToString();
            return new DataObject(jsonString);
        }

        protected virtual void SerializeNestedObject<T>(NestedMapping nestedMapping, T data, JSONLazyNode state)
        {
            var currentData = AttemptGetValue(nestedMapping, data, state);
            if (currentData == null) { return; }
            Serialize(nestedMapping.InternalMappings, currentData, state);
        }

        /*
         * 
        
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

        private JSONNode SerializePrimitive(object value, Type type)
        {
            if (value == null) { return GetNullNode(); }

            var isDefaultPrimitive = MappingRegistry.TypeMapper.TypeAnalyzer.IsDefaultPrimitiveType(type);
            if(isDefaultPrimitive) { return SerializeDefaultPrimitive(value, type); }

            var isNullablePrimitive = MappingRegistry.TypeMapper.TypeAnalyzer.IsNullablePrimitiveType(type);
            if (isNullablePrimitive)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return SerializeDefaultPrimitive(value, underlyingType);
            }
            
            var node = new JSONObject();
            var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
            if(matchingHandler == null) { throw new NoKnownTypeException(type); }
            matchingHandler.HandleTypeSerialization(node, value);
            return node;
        }



        private JSONNode SerializeProperty<T>(PropertyMapping propertyMapping, T data)
        {
            if (data == null) { return GetNullNode(); }
            var underlyingValue = propertyMapping.GetValue(data);
            if (underlyingValue == null) { return GetNullNode(); }
            
            return SerializePrimitive(underlyingValue, propertyMapping.Type);
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
        */
        /*
        protected override void Serialize<T>(IEnumerable<Mapping> mappings, T data, JSONLazyNode state)
        {
            foreach (var mapping in mappings)
            {
                var jsonNode = new JSONLazyNode();
                state.Add(mapping.LocalName, jsonNode);
                
                DelegateMappingType(mapping, data, jsonNode);
            }
        }
    }*/
}