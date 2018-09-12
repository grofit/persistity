using System;
using System.Threading.Tasks;
using LazyData;

namespace Persistity.Endpoints
{
    public interface ISendDataEndpoint
    {
        Task<object> Send(DataObject data);
    }
}