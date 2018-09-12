using System.Threading.Tasks;
using LazyData;

namespace Persistity.Processors
{
    public interface IProcessor
    {
        Task<DataObject> Process(DataObject data);
    }
}