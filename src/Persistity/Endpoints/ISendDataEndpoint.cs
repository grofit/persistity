using System;
using LazyData;

namespace Persistity.Endpoints
{
    public interface ISendDataEndpoint
    {
        void Execute(DataObject data, Action<object> onSuccess, Action<Exception> onError);
    }
}