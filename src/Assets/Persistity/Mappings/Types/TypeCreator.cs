using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Persistity.Mappings.Types
{
    public class TypeCreator : ITypeCreator
    {
        protected readonly Type Dictionarytype = typeof(Dictionary<,>);
        public IDictionary<string, Type> TypeCache { get; private set; }

        public TypeCreator()
        {
            TypeCache = new Dictionary<string, Type>();
        }

        public Type LoadType(string partialName)
        {
            if (TypeCache.ContainsKey(partialName))
            { return TypeCache[partialName]; }

            var type = Type.GetType(partialName) ??
                       AppDomain.CurrentDomain.GetAssemblies()
                           .Select(a => a.GetType(partialName))
                           .FirstOrDefault(t => t != null);

            TypeCache.Add(partialName, type);
            return type;
        }

        public IDictionary CreateDictionary(Type keyType, Type valueType)
        {
            var constructedDictionaryType = Dictionarytype.MakeGenericType(keyType, valueType);
            return (IDictionary)Activator.CreateInstance(constructedDictionaryType);
        }
    }
}