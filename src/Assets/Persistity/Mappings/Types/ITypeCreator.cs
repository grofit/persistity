using System;
using System.Collections;

namespace Persistity.Mappings.Types
{
    public interface ITypeCreator
    {
        Type LoadType(string partialName);
        IDictionary CreateDictionary(Type keyType, Type valueType);
        IList CreateFixedCollection(Type collectionType, int size);
        IList CreateList(Type elementType);
    }
}