using System;

namespace Persistity.Endpoints
{
    public interface IReceiveData
    { }

    public interface IReceiveData<TOut> : IReceiveData
    {
        void Execute(Action<TOut> onSuccess, Action<Exception> onError);
    }

    public interface IReceiveData<TOut, TIn> : IReceiveData
    {
        void Execute(TIn data, Action<TOut> onSuccess, Action<Exception> onError);
    }
}