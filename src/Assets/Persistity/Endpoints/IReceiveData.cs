using System;

namespace Persistity.Endpoints
{
    public interface IReceiveData
    { }

    public interface IReceiveData<TOut> : ISendData
    {
        void Execute(Action<TOut> onSuccess, Action<Exception> onError);
    }

    public interface IReceiveData<TOut, TIn> : ISendData
    {
        void Execute(TIn data, Action<TOut> onSuccess, Action<Exception> onError);
    }

}