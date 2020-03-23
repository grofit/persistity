using System.Collections.Generic;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines
{
    public interface IFlowPipeline : IPipeline
    {
        IEnumerable<IPipelineStep> Steps { get; }
    }
}