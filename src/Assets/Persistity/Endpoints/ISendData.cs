using System;

namespace Persistity.Endpoints
{
    public interface ISendData
    {}

    public interface ISendData<TIn> : ISendData
    {
        void Execute(TIn data, Action onSuccess, Action<Exception> onError);
    }

    public interface ISendData<TIn, TOut> : ISendData
    {
        void Execute(TIn data, Action<TOut> onSuccess, Action<Exception> onError);
    }
}