using Newtonsoft.Json;
using Persistity.Core;
using Persistity.Core.Data;
using Persistity.Core.Serialization;

namespace Persistity.Serializers.Json
{
    public class JsonSerializer : ISerializer
    {
        public DataObject Serialize(object data, object args = null)
        {
            string output;
            if (args != null && args is JsonSerializerSettings settings)
            { output = JsonConvert.SerializeObject(data, settings); }
            else
            { output = JsonConvert.SerializeObject(data); }
            return new DataObject(output);
        }
    }
}