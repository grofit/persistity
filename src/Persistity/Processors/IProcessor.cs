using System.Threading.Tasks;
using Persistity.Core;

namespace Persistity.Processors
{
    public interface IProcessor
    {
        Task<DataObject> Process(DataObject data);
    }
}