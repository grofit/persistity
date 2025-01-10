using System.Threading.Tasks;
using Persistity.Core;
using Persistity.Core.Data;

namespace Persistity.Endpoints
{
    public interface ISendDataEndpoint
    {
        Task<object> Send(DataObject data);
    }
}