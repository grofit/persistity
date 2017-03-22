using Persistity.Registries;
using Persistity.Serialization.Xml;

namespace Persistity.Transformers.Xml
{
    public class XmlTransformer : IXmlTransformer
    {
        public IXmlSerializer Serializer { get; set; }
        public IXmlDeserializer Deserializer { get; set; }
        public IMappingRegistry MappingRegistry { get; set; }

        public XmlTransformer(IXmlSerializer serializer, IXmlDeserializer deserializer, IMappingRegistry mappingRegistry)
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