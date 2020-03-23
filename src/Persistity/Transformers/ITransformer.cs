namespace Persistity.Transformers
{
    public interface ITransformer
    {
        object Transform(object original);
    }
}