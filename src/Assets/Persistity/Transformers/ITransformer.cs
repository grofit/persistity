namespace Persistity.Transformers
{
    public interface ITransformer
    {
        byte[] Transform<T>(T data) where T : new();
        T Transform<T>(byte[] data) where T : new();
    }
}