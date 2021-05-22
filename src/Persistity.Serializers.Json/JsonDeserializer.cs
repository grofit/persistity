using System;
using Newtonsoft.Json;
using Persistity.Core;
using Persistity.Core.Serialization;

namespace Persistity.Serializers.Json
{
    public class JsonDeserializer : IDeserializer
    {
        public object Deserialize(DataObject data, Type type, object args = null)
        {
            var settings = args == null ? new JsonSerializerSettings() : (JsonSerializerSettings)args;
            return JsonConvert.DeserializeObject(data.AsString, type, settings);
        }
    }
}