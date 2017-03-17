using Persistity.Mappings;

namespace Persistity.Serialization
{
    public interface IDeserializer<TInput>
    {
        TOutput DeserializeData<TOutput>(TypeMapping typeMapping, TInput data) where TOutput : new();
    }
}