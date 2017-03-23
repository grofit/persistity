using Persistity.Registries;
using Persistity.Serialization.Binary;

namespace Persistity.Transformers.Binary
{
    public class BinaryTransformer : IBinaryTransformer
    {
        public IBinarySerializer Serializer { get; set; }
        public IBinaryDeserializer Deserializer { get; set; }
        public IMappingRegistry MappingRegistry { get; set; }

        public BinaryTransformer(IBinarySerializer serializer, IBinaryDeserializer deserializer, IMappingRegistry mappingRegistry)
        {
            Serializer = serializer;
            Deserializer = deserializer;
            MappingRegistry = mappingRegistry;
        }

        public byte[] Transform<T>(T data) where T : new()
        {
            var typeMapping = MappingRegistry.GetMappingFor<T>();
            return Serializer.SerializeData(typeMapping, data);
        }

        public T Transform<T>(byte[] data) where T : new()
        {
            var typeMapping = MappingRegistry.GetMappingFor<T>();
            return Deserializer.DeserializeData<T>(typeMapping, data);
        }
    }
}