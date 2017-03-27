using System;

namespace Persistity.Pipelines
{
    public interface IReceiveDataPipeline
    {
        void Execute<TDataType>(Action<object> onSuccess, Action<Exception> onError) where TDataType : new();
    }
}