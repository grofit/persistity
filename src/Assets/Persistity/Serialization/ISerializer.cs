using Persistity.Mappings;

namespace Persistity.Serialization
{
    public interface ISerializer<TOutput>
    {
        TOutput SerializeData<TInput>(TypeMapping typeMapping, TInput data) where TInput : new();
    }
}