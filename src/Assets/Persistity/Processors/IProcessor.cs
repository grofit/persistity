namespace Persistity.Processors
{
    public interface IProcessor<T>
    {
        T Process(T data);
    }
}