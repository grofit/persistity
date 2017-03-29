using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Persistity.Attributes;
using Persistity.Extensions;
using UnityEngine;

namespace Persistity.Mappings.Mappers
{
    public abstract class TypeMapper : ITypeMapper
    {
        public MappingConfiguration Configuration { get; private set; }
        public IDictionary<string, Type> TypeCache { get; private set; }

        protected TypeMapper(MappingConfiguration configuration = null)
        {
            Configuration = configuration ?? MappingConfiguration.Default;
            TypeCache = new Dictionary<string, Type>();
        }

        public bool IsGenericList(Type type)
        { return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>); }

        public bool IsGenericDictionary(Type type)
        { return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>); }

        public bool IsDynamicType(Type type)
        { return type.IsAbstract || type.IsInterface || type == typeof(object); }

        public bool IsDynamicType(PropertyInfo propertyInfo)
        {
            var typeIsDynamic = IsDynamicType(propertyInfo.PropertyType);
            if(typeIsDynamic) { return true; }

            return propertyInfo.HasAttribute<DynamicTypeAttribute>();
        }

        public Type LoadType(string partialName)
        {
            if(TypeCache.ContainsKey(partialName))
            { return TypeCache[partialName]; }

            var type = Type.GetType(partialName) ??
            AppDomain.CurrentDomain.GetAssemblies()
                    .Select(a => a.GetType(partialName))
                    .FirstOrDefault(t => t != null);

            TypeCache.Add(partialName, type);
            return type;
        }

        public virtual bool IsPrimitiveType(Type type)
        {
            var isDefaultPrimitive = type.IsPrimitive ||
                   type == typeof(string) ||
                   type == typeof(DateTime) ||
                   type == typeof(Vector2) ||
                   type == typeof(Vector3) ||
                   type == typeof(Vector4) ||
                   type == typeof(Quaternion) ||
                   type == typeof(Guid) ||
                   type.IsEnum;

            return isDefaultPrimitive || Configuration.KnownPrimitives.Any(x => type == x);
        }

        public virtual TypeMapping GetTypeMappingsFor(Type type)
        {
            var typeMapping = new TypeMapping
            {
                Name = type.FullName,
                Type = type
            };

            var mappings = GetMappingsFromType(type, type.Name);
            typeMapping.InternalMappings.AddRange(mappings);

            return typeMapping;
        }

        public virtual List<Mapping> GetMappingsFromType(Type type, string scope)
        {
            var properties = GetPropertiesFor(type);

            if (Configuration.IgnoredTypes.Any())
            {
                properties = properties.Where(
                    x => !Configuration.IgnoredTypes.Any(y => x.PropertyType.IsAssignableFrom(y)));
            }

            return properties.Select(propertyInfo => GetMappingFor(propertyInfo, scope)).ToList();
        }

        public virtual IEnumerable<PropertyInfo> GetPropertiesFor(Type type)
        {
            return type.GetProperties()
                .Where(x => x.CanRead && x.CanWrite);
        }

        public virtual Mapping GetMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var currentScope = scope + "." + propertyInfo.Name;

            if (IsPrimitiveType(propertyInfo.PropertyType))
            { return CreatePropertyMappingFor(propertyInfo, currentScope); }

            if (propertyInfo.PropertyType.IsArray || IsGenericList(propertyInfo.PropertyType))
            { return CreateCollectionMappingFor(propertyInfo, currentScope); }

            if (IsGenericDictionary(propertyInfo.PropertyType))
            { return CreateDictionaryMappingFor(propertyInfo, currentScope); }

            return CreateNestedMappingFor(propertyInfo, currentScope);
        }

        public virtual CollectionMapping CreateCollectionMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var propertyType = propertyInfo.PropertyType;
            var isArray = propertyType.IsArray;
            var collectionType = isArray ? propertyType.GetElementType() : propertyType.GetGenericArguments()[0];

            var collectionMapping = new CollectionMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                CollectionType = collectionType,
                Type = propertyInfo.PropertyType,
                GetValue = (x) => propertyInfo.GetValue(x, null) as IList,
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null),
                IsArray = isArray,
                IsElementDynamicType = IsDynamicType(collectionType)
            };

            var collectionMappingTypes = GetMappingsFromType(collectionType, scope);
            collectionMapping.InternalMappings.AddRange(collectionMappingTypes);

            return collectionMapping;
        }

        public virtual DictionaryMapping CreateDictionaryMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var propertyType = propertyInfo.PropertyType;
            var dictionaryTypes = propertyType.GetGenericArguments();

            var keyType = dictionaryTypes[0];
            var valueType = dictionaryTypes[1];

            var dictionaryMapping = new DictionaryMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                KeyType = keyType,
                ValueType = valueType,
                Type = propertyInfo.PropertyType,
                GetValue = (x) => propertyInfo.GetValue(x, null) as IDictionary,
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null),
                IsKeyDynamicType = IsDynamicType(keyType),
                IsValueDynamicType = IsDynamicType(valueType)
            };

            var keyMappingTypes = GetMappingsFromType(keyType, scope);
            dictionaryMapping.KeyMappings.AddRange(keyMappingTypes);

            var valueMappingTypes = GetMappingsFromType(valueType, scope);
            dictionaryMapping.ValueMappings.AddRange(valueMappingTypes);

            return dictionaryMapping;
        }

        public virtual PropertyMapping CreatePropertyMappingFor(PropertyInfo propertyInfo, string scope)
        {
            return new PropertyMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                Type = propertyInfo.PropertyType,
                GetValue = x => propertyInfo.GetValue(x, null),
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null)
            };
        }

        public virtual NestedMapping CreateNestedMappingFor(PropertyInfo propertyInfo, string scope)
        {
            var nestedMapping = new NestedMapping
            {
                LocalName = propertyInfo.Name,
                ScopedName = scope,
                Type = propertyInfo.PropertyType,
                GetValue = x => propertyInfo.GetValue(x, null),
                SetValue = (x, v) => propertyInfo.SetValue(x, v, null),
                IsDynamicType = IsDynamicType(propertyInfo)
            };

            var mappingTypes = GetMappingsFromType(propertyInfo.PropertyType, scope);
            nestedMapping.InternalMappings.AddRange(mappingTypes);
            return nestedMapping;
        }

        public virtual T GetKey<T, K>(IDictionary<T, K> dictionary, int index)
        { return dictionary.Keys.ElementAt(index); }

        public virtual K GetValue<T, K>(IDictionary<T, K> dictionary, T key)
        { return dictionary[key]; }
    }
}