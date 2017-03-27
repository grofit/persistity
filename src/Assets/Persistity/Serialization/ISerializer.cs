namespace Persistity.Serialization
{
    public interface ISerializer
    {
        DataObject SerializeData(object data);
    }
}