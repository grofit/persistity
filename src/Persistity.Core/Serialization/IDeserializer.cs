using System;
using Persistity.Core.Data;

namespace Persistity.Core.Serialization
{
    public interface IDeserializer
    {
        object Deserialize(DataObject data, Type type, object args = null);
    }
}