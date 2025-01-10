using System;
using Persistity.Core;
using Persistity.Core.Data;
using Persistity.Core.Serialization;
using LazyDataDeserializer = LazyData.Json.JsonDeserializer;
using LazyDataObject = LazyData.DataObject;

namespace Persistity.Serializers.LazyData.Json
{
    public class JsonDeserializer : IDeserializer
    {
        public LazyDataDeserializer InternalDeserializer { get; }

        public JsonDeserializer(LazyDataDeserializer internalDeserializer)
        { InternalDeserializer = internalDeserializer; }

        public object Deserialize(DataObject data, Type type, object args = null)
        {
            var lazyDataObject = new LazyDataObject(data.AsBytes);
            return InternalDeserializer.Deserialize(lazyDataObject, type);
        }
    }
}