using Persistity.Core;
using Persistity.Core.Serialization;
using LazyDataSerializer = LazyData.Json.JsonSerializer;

namespace Persistity.Serializers.LazyData.Json
{
    public class JsonSerializer : ISerializer
    {
        public LazyDataSerializer InternalSerializer { get; }

        public JsonSerializer(LazyDataSerializer internalSerializer)
        { InternalSerializer = internalSerializer; }

        public DataObject Serialize(object data, object args = null)
        {
            var persistTypes = (bool?) args ?? false;
            var outputData = InternalSerializer.Serialize(data, persistTypes);
            return new DataObject(outputData.AsBytes);
        }
    }
}