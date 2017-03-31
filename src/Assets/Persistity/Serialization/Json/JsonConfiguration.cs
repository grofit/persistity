using Persistity.Json;

namespace Persistity.Serialization.Json
{
    public class JsonConfiguration : SerializationConfiguration<JSONLazyNode, JSONNode>
    {
        public static JsonConfiguration Default
        {
            get
            {
                return new JsonConfiguration
                {
                    TypeHandlers = new ITypeHandler<JSONLazyNode, JSONNode>[0]
                };
            }
        }
    }
}