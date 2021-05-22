using Persistity.Core;
using Persistity.Core.Serialization;
using LazyDataSerializer = LazyData.Binary.BinarySerializer;

namespace Persistity.Serializers.LazyData.Binary
{
    public class BinarySerializer : ISerializer
    {
        public LazyDataSerializer InternalSerializer { get; }

        public BinarySerializer(LazyDataSerializer internalSerializer)
        { InternalSerializer = internalSerializer; }

        public DataObject Serialize(object data, object args = null)
        {
            var persistTypes = (bool?) args ?? false;
            var outputData = InternalSerializer.Serialize(data, persistTypes);
            return new DataObject(outputData.AsBytes);
        }
    }
}