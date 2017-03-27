namespace Persistity.Serialization
{
    public interface IDeserializer
    {
        object DeserializeData(DataObject data);
        T DeserializeData<T>(DataObject data) where T : new();
    }
}