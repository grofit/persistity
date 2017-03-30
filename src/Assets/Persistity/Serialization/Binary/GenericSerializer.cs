using System;
using System.Collections.Generic;
using System.Linq;
using Persistity.Exceptions;
using Persistity.Mappings;
using Persistity.Registries;

namespace Persistity.Serialization.Binary
{
    public abstract class GenericSerializer<TSerializeState, TDeserializeState> : ISerializer
    {
        public IMappingRegistry MappingRegistry { get; private set; }
        public SerializationConfiguration<TSerializeState, TDeserializeState> Configuration { get; protected set; }

        protected GenericSerializer(IMappingRegistry mappingRegistry, SerializationConfiguration<TSerializeState, TDeserializeState> configuration = null)
        {
            MappingRegistry = mappingRegistry;
            Configuration = configuration ?? SerializationConfiguration<TSerializeState, TDeserializeState>.Default;
        }

        public abstract void HandleNullData(TSerializeState state);
        public abstract void HandleNullObject(TSerializeState state);
        public abstract void AddCountToState(TSerializeState state, int count);
        public abstract void SerializeDefaultPrimitive(object value, Type type, TSerializeState state);
        public abstract DataObject Serialize(object data);

        public void SerializePrimitive(object value, Type type, TSerializeState state)
        {
            if (value == null)
            {
                HandleNullData(state);
                return;
            }

            var isDefaultPrimitive = MappingRegistry.TypeMapper.TypeAnalyzer.IsDefaultPrimitiveType(type);
            if (isDefaultPrimitive)
            {
                SerializeDefaultPrimitive(value, type, state);
                return;
            }

            var isNullablePrimitive = MappingRegistry.TypeMapper.TypeAnalyzer.IsNullablePrimitiveType(type);
            if (isNullablePrimitive)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                SerializeDefaultPrimitive(value, underlyingType, state);
                return;
            }

            var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
            if(matchingHandler == null) { throw new NoKnownTypeException(type); }
            matchingHandler.HandleTypeSerialization(state, value);
        }

        public void SerializeProperty<T>(PropertyMapping propertyMapping, T data, TSerializeState state)
        {
            if (data == null)
            {
                HandleNullData(state);
                return;
            }

            var underlyingValue = propertyMapping.GetValue(data);

            if (underlyingValue == null)
            {
                HandleNullData(state);
                return;
            }

            SerializePrimitive(underlyingValue, propertyMapping.Type, state);
        }

        public void SerializeNestedObject<T>(NestedMapping nestedMapping, T data, TSerializeState state)
        {
            if (data == null)
            {
                HandleNullObject(state);
                return;
            }

            var currentData = nestedMapping.GetValue(data);

            if (currentData == null)
            {
                HandleNullObject(state);
                return;
            }

            Serialize(nestedMapping.InternalMappings, currentData, state);
        }
        
        public void SerializeCollection<T>(CollectionMapping collectionMapping, T data, TSerializeState state)
        {
            if (data == null)
            {
                HandleNullObject(state);
                return;
            }

            var collectionValue = collectionMapping.GetValue(data);

            if (collectionValue == null)
            {
                HandleNullObject(state);
                return;
            }

            AddCountToState(state, collectionValue.Count);
            for (var i = 0; i < collectionValue.Count; i++)
            {
                var currentData = collectionValue[i];
                if (currentData == null)
                { HandleNullObject(state); }
                else if (collectionMapping.InternalMappings.Count > 0)
                { Serialize(collectionMapping.InternalMappings, currentData, state); }
                else
                { SerializePrimitive(currentData, collectionMapping.CollectionType, state); }
            }
        }

        public void SerializeDictionary<T>(DictionaryMapping dictionaryMapping, T data, TSerializeState state)
        {
            if (data == null)
            {
                HandleNullObject(state);
                return;
            }

            var dictionaryValue = dictionaryMapping.GetValue(data);

            if (dictionaryValue == null)
            {
                HandleNullObject(state);
                return;
            }

            AddCountToState(state, dictionaryValue.Count);

            foreach (var key in dictionaryValue.Keys)
            {
                var currentValue = dictionaryValue[key];

                if (dictionaryMapping.KeyMappings.Count > 0)
                { Serialize(dictionaryMapping.KeyMappings, key, state); }
                else
                { SerializePrimitive(key, dictionaryMapping.KeyType, state); }

                if (currentValue == null)
                { HandleNullData(state); }
                else if (dictionaryMapping.ValueMappings.Count > 0)
                { Serialize(dictionaryMapping.ValueMappings, currentValue, state); }
                else
                { SerializePrimitive(currentValue, dictionaryMapping.ValueType, state); }
            }
        }

        public void Serialize<T>(IEnumerable<Mapping> mappings, T data, TSerializeState state)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                { SerializeProperty((mapping as PropertyMapping), data, state); }
                else if (mapping is NestedMapping)
                { SerializeNestedObject((mapping as NestedMapping), data, state); }
                else if(mapping is DictionaryMapping)
                { SerializeDictionary(mapping as DictionaryMapping, data, state);}
                else
                { SerializeCollection((mapping as CollectionMapping), data, state); }
            }
        }
    }
}