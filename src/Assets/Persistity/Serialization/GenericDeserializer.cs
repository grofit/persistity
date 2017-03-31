using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Persistity.Mappings;
using Persistity.Registries;

namespace Persistity.Serialization
{
    public abstract class GenericDeserializer<TSerializeState, TDeserializeState> : IDeserializer
    {
        public IMappingRegistry MappingRegistry { get; private set; }
        public SerializationConfiguration<TSerializeState, TDeserializeState> Configuration { get; protected set; }

        protected GenericDeserializer(IMappingRegistry mappingRegistry, SerializationConfiguration<TSerializeState, TDeserializeState> configuration = null)
        {
            MappingRegistry = mappingRegistry;
            Configuration = configuration ?? SerializationConfiguration<TSerializeState, TDeserializeState>.Default;
        }

        public abstract object Deserialize(DataObject data);
        public abstract T Deserialize<T>(DataObject data) where T : new();
        protected abstract bool IsDataNull(TDeserializeState state);
        protected abstract bool IsObjectNull(TDeserializeState state);
        protected abstract int GetCountFromState(TDeserializeState state);
        protected abstract object DeserializeDefaultPrimitive(Type type, TDeserializeState state);

        protected void DeserializeProperty<T>(PropertyMapping propertyMapping, T instance, TDeserializeState state)
        {
            if (IsDataNull(state))
            { propertyMapping.SetValue(instance, null); }
            else
            {
                var underlyingValue = DeserializePrimitive(propertyMapping.Type, state);
                propertyMapping.SetValue(instance, underlyingValue);
            }
        }

        protected void DeserializeCollection(CollectionMapping collectionMapping, IList collectionInstance, int count, TDeserializeState state)
        {
            for (var i = 0; i < count; i++)
            {
                if (IsObjectNull(state))
                {
                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = null; }
                    else
                    { collectionInstance.Insert(i, null); }
                }
                else if (collectionMapping.InternalMappings.Count > 0)
                {
                    var elementInstance = Activator.CreateInstance(collectionMapping.CollectionType);
                    Deserialize(collectionMapping.InternalMappings, elementInstance, state);

                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = elementInstance; }
                    else
                    { collectionInstance.Insert(i, elementInstance); }
                }
                else
                {
                    object value = null;
                    if (!IsDataNull(state))
                    { value = DeserializePrimitive(collectionMapping.CollectionType, state); }
                    if (collectionInstance.IsFixedSize)
                    { collectionInstance[i] = value; }
                    else
                    { collectionInstance.Insert(i, value); }
                }
            }
        }

        protected virtual object DeserializePrimitive(Type type, TDeserializeState state)
        {
            if (IsDataNull(state))
            { return null; }

            var isDefaultPrimitive = MappingRegistry.TypeMapper.TypeAnalyzer.IsDefaultPrimitiveType(type);
            if (isDefaultPrimitive)
            { return DeserializeDefaultPrimitive(type, state); }

            var isNullablePrimitive = MappingRegistry.TypeMapper.TypeAnalyzer.IsNullablePrimitiveType(type);
            if (isNullablePrimitive)
            {
                var underlyingType = Nullable.GetUnderlyingType(type);
                return DeserializeDefaultPrimitive(underlyingType, state);
            }

            var matchingHandler = Configuration.TypeHandlers.SingleOrDefault(x => x.MatchesType(type));
            if (matchingHandler != null)
            { return matchingHandler.HandleTypeDeserialization(state); }

            throw new Exception("Type is not primitive or known, cannot deserialize " + type);
        }

        protected virtual void DeserializeDictionary(DictionaryMapping dictionaryMapping, IDictionary dictionaryInstance, int count, TDeserializeState state)
        {
            for (var i = 0; i < count; i++)
            {
                object keyInstance, valueInstance;
                if (dictionaryMapping.KeyMappings.Count > 0)
                {
                    keyInstance = Activator.CreateInstance(dictionaryMapping.KeyType);
                    Deserialize(dictionaryMapping.KeyMappings, keyInstance, state);
                }
                else
                { keyInstance = DeserializePrimitive(dictionaryMapping.KeyType, state); }

                if (IsDataNull(state))
                { valueInstance = null; }
                else if (dictionaryMapping.ValueMappings.Count > 0)
                {
                    valueInstance = Activator.CreateInstance(dictionaryMapping.ValueType);
                    Deserialize(dictionaryMapping.ValueMappings, valueInstance, state);
                }
                else
                { valueInstance = DeserializePrimitive(dictionaryMapping.ValueType, state); }

                dictionaryInstance.Add(keyInstance, valueInstance);
            }
        }

        protected void DeserializeNestedObject<T>(NestedMapping nestedMapping, T instance, TDeserializeState state)
        { Deserialize(nestedMapping.InternalMappings, instance, state); }

        public void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, TDeserializeState state)
        {
            foreach (var mapping in mappings)
            {
                if (mapping is PropertyMapping)
                { DeserializeProperty((mapping as PropertyMapping), instance, state); }
                else if (mapping is NestedMapping)
                {
                    var nestedMapping = (mapping as NestedMapping);
                    if (IsObjectNull(state))
                    {
                        nestedMapping.SetValue(instance, null);
                        continue;
                    }

                    var childInstance = Activator.CreateInstance(nestedMapping.Type);
                    DeserializeNestedObject(nestedMapping, childInstance, state);
                    nestedMapping.SetValue(instance, childInstance);
                }
                else if (mapping is DictionaryMapping)
                {
                    var dictionaryMapping = (mapping as DictionaryMapping);
                    if (IsObjectNull(state))
                    {
                        dictionaryMapping.SetValue(instance, null);
                        continue;
                    }

                    var dictionarytype = typeof(Dictionary<,>);
                    var dictionaryCount = GetCountFromState(state);
                    var constructedDictionaryType = dictionarytype.MakeGenericType(dictionaryMapping.KeyType, dictionaryMapping.ValueType);
                    var dictionary = (IDictionary)Activator.CreateInstance(constructedDictionaryType);
                    DeserializeDictionary(dictionaryMapping, dictionary, dictionaryCount, state);
                    dictionaryMapping.SetValue(instance, dictionary);
                }
                else
                {
                    var collectionMapping = (mapping as CollectionMapping);
                    if (IsObjectNull(state))
                    {
                        collectionMapping.SetValue(instance, null);
                        continue;
                    }

                    var arrayCount = GetCountFromState(state);

                    if (collectionMapping.IsArray)
                    {
                        var arrayInstance = (IList)Activator.CreateInstance(collectionMapping.Type, arrayCount);
                        DeserializeCollection(collectionMapping, arrayInstance, arrayCount, state);
                        collectionMapping.SetValue(instance, arrayInstance);
                    }
                    else
                    {
                        var listType = typeof(List<>);
                        var constructedListType = listType.MakeGenericType(collectionMapping.CollectionType);
                        var listInstance = (IList)Activator.CreateInstance(constructedListType);
                        DeserializeCollection(collectionMapping, listInstance, arrayCount, state);
                        collectionMapping.SetValue(instance, listInstance);
                    }
                }
            }
        }
    }
}