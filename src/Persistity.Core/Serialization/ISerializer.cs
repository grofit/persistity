namespace Persistity.Core.Serialization
{
    public interface ISerializer
    {
        DataObject Serialize(object data, object args = null);
    }
}