using System;

namespace Persistity.Endpoints
{
    public interface IReceiveData
    {
        void Execute(Action<byte[]> onSuccess, Action<Exception> onError);
    }
}