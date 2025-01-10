using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Data;

namespace Persistity.Processors
{
    public interface IProcessor
    {
        Task<DataObject> Process(DataObject data);
    }
}