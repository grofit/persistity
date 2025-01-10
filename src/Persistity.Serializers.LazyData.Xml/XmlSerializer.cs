using Persistity.Core;
using Persistity.Core.Data;
using Persistity.Core.Serialization;
using LazyDataSerializer = LazyData.Xml.XmlSerializer;

namespace Persistity.Serializers.LazyData.Xml
{
    public class XmlSerializer : ISerializer
    {
        public LazyDataSerializer InternalSerializer { get; }

        public XmlSerializer(LazyDataSerializer internalSerializer)
        { InternalSerializer = internalSerializer; }

        public DataObject Serialize(object data, object args = null)
        {
            var persistTypes = (bool?) args ?? false;
            var outputData = InternalSerializer.Serialize(data, persistTypes);
            return new DataObject(outputData.AsBytes);
        }
    }
}