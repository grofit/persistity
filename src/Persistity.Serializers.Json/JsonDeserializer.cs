using System;
using Newtonsoft.Json;
using Persistity.Core;
using Persistity.Core.Data;
using Persistity.Core.Serialization;

namespace Persistity.Serializers.Json
{
    public class JsonDeserializer : IDeserializer
    {
        public object Deserialize(DataObject data, Type type, object args = null)
        {
            if (args != null && args is JsonSerializerSettings settings)
            { return JsonConvert.DeserializeObject(data.AsString, type, settings); }

            return JsonConvert.DeserializeObject(data.AsString, type);
        }
    }
}