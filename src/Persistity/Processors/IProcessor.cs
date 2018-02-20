using LazyData;

namespace Persistity.Processors
{
    public interface IProcessor
    {
        DataObject Process(DataObject data);
    }
}