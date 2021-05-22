using System;
using Persistity.Core;
using Persistity.Core.Serialization;
using LazyDataDeserializer = LazyData.Xml.XmlDeserializer;
using LazyDataObject = LazyData.DataObject;

namespace Persistity.Serializers.LazyData.Xml
{
    public class XmlDeserializer : IDeserializer
    {
        public LazyDataDeserializer InternalDeserializer { get; }

        public XmlDeserializer(LazyDataDeserializer internalDeserializer)
        { InternalDeserializer = internalDeserializer; }

        public object Deserialize(DataObject data, Type type, object args = null)
        {
            var lazyDataObject = new LazyDataObject(data.AsBytes);
            return InternalDeserializer.Deserialize(lazyDataObject, type);
        }
    }
}