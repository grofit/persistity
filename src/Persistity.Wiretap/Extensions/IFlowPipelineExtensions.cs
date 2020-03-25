using Persistity.Flow.Pipelines;
using Persistity.Wiretap.Pipelines;

namespace Persistity.Wiretap.Extensions
{
    public static class IFlowPipelineExtensions
    {
        public static IWireTappablePipeline AsWireTappable(this IFlowPipeline pipeline)
        { return new WireTappablePipeline(pipeline.Steps); }
    }
}