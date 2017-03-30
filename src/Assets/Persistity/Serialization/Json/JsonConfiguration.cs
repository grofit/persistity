using Persistity.Json;

namespace Persistity.Serialization.Json
{
    public class JsonConfiguration : SerializationConfiguration<JSONNode, JSONNode>
    {
        public static JsonConfiguration Default
        {
            get
            {
                return new JsonConfiguration
                {
                    TypeHandlers = new ITypeHandler<JSONNode, JSONNode>[0]
                };
            }
        }
    }
}