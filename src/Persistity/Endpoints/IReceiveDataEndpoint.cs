using System;
using LazyData;

namespace Persistity.Endpoints
{
    public interface IReceiveDataEndpoint
    {
        void Execute(Action<DataObject> onSuccess, Action<Exception> onError);
    }
}