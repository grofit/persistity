using System;
using System.Threading.Tasks;
using LazyData;

namespace Persistity.Endpoints
{
    public interface IReceiveDataEndpoint
    {
        Task<DataObject> Receive();
    }
}