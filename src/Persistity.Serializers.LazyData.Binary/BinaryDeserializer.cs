using System;
using Persistity.Core;
using Persistity.Core.Serialization;
using LazyDataDeserializer = LazyData.Binary.BinaryDeserializer;
using LazyDataObject = LazyData.DataObject;

namespace Persistity.Serializers.LazyData.Binary
{
    public class BinaryDeserializer : IDeserializer
    {
        public LazyDataDeserializer InternalDeserializer { get; }

        public BinaryDeserializer(LazyDataDeserializer internalDeserializer)
        { InternalDeserializer = internalDeserializer; }

        public object Deserialize(DataObject data, Type type, object args = null)
        {
            var lazyDataObject = new LazyDataObject(data.AsBytes);
            return InternalDeserializer.Deserialize(lazyDataObject, type);
        }
    }
}