using System;

namespace Persistity.Endpoints
{
    public interface ISendData
    {
        void Execute(byte[] data, Action onSuccess, Action<Exception> onError);
    }
}