namespace Persistity.Transformers
{
    public interface ITransformer<TOutput>
    {
        TOutput Transform<T>(T data) where T : new();
        T Transform<T>(TOutput data) where T : new();
    }
}