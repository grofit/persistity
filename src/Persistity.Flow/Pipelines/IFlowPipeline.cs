using System.Collections.Generic;
using Persistity.Flow.Steps.Types;
using Persistity.Pipelines;

namespace Persistity.Flow.Pipelines
{
    public interface IFlowPipeline : IPipeline
    {
        IEnumerable<IPipelineStep> Steps { get; }
    }
}