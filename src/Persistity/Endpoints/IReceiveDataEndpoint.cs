using System;
using System.Threading.Tasks;

using Persistity.Core;

namespace Persistity.Endpoints
{
    public interface IReceiveDataEndpoint
    {
        Task<DataObject> Receive();
    }
}