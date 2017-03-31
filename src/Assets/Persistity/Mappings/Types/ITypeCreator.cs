using System;
using System.Collections;

namespace Persistity.Mappings.Types
{
    public interface ITypeCreator
    {
        Type LoadType(string partialName);
        IDictionary CreateDictionary(Type keyType, Type valueType);
    }
}