using System;
using System.Threading.Tasks;

using Persistity.Core;
using Persistity.Core.Data;

namespace Persistity.Endpoints
{
    public interface IReceiveDataEndpoint
    {
        Task<DataObject> Receive();
    }
}