using System;
using System.Collections.Generic;
using Persistity.Mappings;

namespace Persistity.Registries
{
    public class MappingRegistry : IMappingRegistry
    {
        public TypeMapper TypeMapper { get; private set; }
        public IDictionary<Type, TypeMapping> TypeMappings { get; private set; }

        public MappingRegistry(TypeMapper typeMapper)
        {
            TypeMapper = typeMapper;
            TypeMappings = new Dictionary<Type, TypeMapping>();
        }

        public TypeMapping GetMappingFor<T>() where T : new()
        {
            var type = typeof(T);
            if(TypeMappings.ContainsKey(type))
            { return TypeMappings[type]; }

            var typeMapping = TypeMapper.GetTypeMappingsFor(type);
            TypeMappings.Add(type, typeMapping);
            return typeMapping;
        }
    }
}