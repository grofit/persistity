using System;
using Persistity.Flow.Pipelines;
using Persistity.Wiretap.Models;

namespace Persistity.Wiretap.Pipelines
{
    public interface IWireTappablePipeline : IFlowPipeline
    {
        WireTapSubscription StartWiretap(int step, Action<object, object> action);
        void StopWiretap(int step, Action<object, object> action);
    }
}