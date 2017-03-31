using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Persistity.Mappings;
using Persistity.Mappings.Types;
using Persistity.Registries;

namespace Persistity.Serialization
{
    public abstract class GenericDeserializer<TSerializeState, TDeserializeState> : IDeserializer
    {
        public IMappingRegistry MappingRegistry { get; private set; }
        public ITypeCreator TypeCreator { get; private set; }
        public SerializationConfiguration<TSerializeState, TDeserializeState> Configuration { get; protected set; }

        protected GenericDeserializer(IMappingRegistry mappingRegistry, ITypeCreator typeCreator, SerializationConfiguration<TSerializeState, TDeserializeState> configuration = null)
        {
            MappingRegistry = mappingRegistry;
            TypeCreator = typeCreator;
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

        protected void DeserializeNestedObject<T>(NestedMapping nestedMapping, T instance, TDeserializeState state)
        {
            if (IsObjectNull(state))
            {
                nestedMapping.SetValue(instance, null);
                return;
            }

            var childInstance = Activator.CreateInstance(nestedMapping.Type);
            nestedMapping.SetValue(instance, childInstance);
            Deserialize(nestedMapping.InternalMappings, childInstance, state);
        }

        protected void DeserializeCollection<T>(CollectionMapping mapping, T instance, TDeserializeState state)
        {
            if (IsObjectNull(state))
            {
                mapping.SetValue(instance, null);
                return;
            }

            var count = GetCountFromState(state);

            IList collectionInstance;
            if (mapping.IsArray)
            { collectionInstance = TypeCreator.CreateFixedCollection(mapping.Type, count); }
            else
            { collectionInstance = TypeCreator.CreateList(mapping.CollectionType); }

            mapping.SetValue(instance, collectionInstance);

            for (var i = 0; i < count; i++)
            {
                var elementInstance = DeserializeCollectionElement(mapping, state);

                if (collectionInstance.IsFixedSize)
                { collectionInstance[i] = elementInstance; }
                else
                { collectionInstance.Insert(i, elementInstance); }

            }
        }

        protected virtual object DeserializeCollectionElement(CollectionMapping mapping, TDeserializeState state)
        {
            if (IsObjectNull(state))
            { return null; }

            if (mapping.InternalMappings.Count > 0)
            {
                var elementInstance = Activator.CreateInstance(mapping.CollectionType);
                Deserialize(mapping.InternalMappings, elementInstance, state);
                return elementInstance;
            }

            return DeserializePrimitive(mapping.CollectionType, state);
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

        protected virtual void DeserializeDictionary<T>(DictionaryMapping mapping, T instance, TDeserializeState state)
        {
            if (IsObjectNull(state))
            {
                mapping.SetValue(instance, null);
                return;
            }

            var count = GetCountFromState(state);
            var dictionary = TypeCreator.CreateDictionary(mapping.KeyType, mapping.ValueType);
            mapping.SetValue(instance, dictionary);

            for (var i = 0; i < count; i++)
            { DeserializeDictionaryKeyValuePair(mapping, dictionary, state); }
        }

        protected virtual void DeserializeDictionaryKeyValuePair(DictionaryMapping mapping, IDictionary dictionary, TDeserializeState state)
        {
            var keyInstance = DeserializeDictionaryKey(mapping, state);
            var valueInstance = DeserializeDictionaryValue(mapping, state);
            dictionary.Add(keyInstance, valueInstance);
        }

        protected virtual object DeserializeDictionaryKey(DictionaryMapping mapping, TDeserializeState state)
        {
            if (IsDataNull(state))
            { return null; }

            if (mapping.KeyMappings.Count > 0)
            {
                var keyInstance = Activator.CreateInstance(mapping.KeyType);
                Deserialize(mapping.KeyMappings, keyInstance, state);
                return keyInstance;
            }

            return DeserializePrimitive(mapping.KeyType, state);
        }

        protected virtual object DeserializeDictionaryValue(DictionaryMapping mapping, TDeserializeState state)
        {
            if (IsDataNull(state))
            { return null; }

            if (mapping.ValueMappings.Count > 0)
            {
                var valueInstance = Activator.CreateInstance(mapping.ValueType);
                Deserialize(mapping.ValueMappings, valueInstance, state);
                return valueInstance;
            }

            return DeserializePrimitive(mapping.ValueType, state);
        }

        protected virtual void Deserialize<T>(IEnumerable<Mapping> mappings, T instance, TDeserializeState state)
        {
            foreach (var mapping in mappings)
            { DelegateMappingType(mapping, instance, state); }
        }

        protected virtual void DelegateMappingType<T>(Mapping mapping, T instance, TDeserializeState state)
        {
            if (mapping is PropertyMapping)
            { DeserializeProperty((mapping as PropertyMapping), instance, state); }
            else if (mapping is NestedMapping)
            { DeserializeNestedObject((mapping as NestedMapping), instance, state); }
            else if (mapping is DictionaryMapping)
            { DeserializeDictionary(mapping as DictionaryMapping, instance, state); }
            else
            { DeserializeCollection((mapping as CollectionMapping), instance, state); }
        }
    }
}