using System.Threading.Tasks;
using Persistity.Core;

namespace Persistity.Endpoints
{
    public interface ISendDataEndpoint
    {
        Task<object> Send(DataObject data);
    }
}