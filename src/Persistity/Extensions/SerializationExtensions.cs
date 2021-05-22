using Persistity.Core;
using Persistity.Core.Serialization;

namespace Persistity.Extensions
{
    public static class SerializationExtensions
    {
        public static T Deserialize<T>(this IDeserializer deserializer, DataObject data, object args = null)
        { return (T) deserializer.Deserialize(data, typeof(T), args); }
    }
}