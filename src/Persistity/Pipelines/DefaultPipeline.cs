using System.Collections.Generic;
using Persistity.Pipelines.Steps.Types;

namespace Persistity.Pipelines
{
    public class DefaultPipeline : FlowPipeline
    {
        public DefaultPipeline(IEnumerable<IPipelineStep> steps)
        { Steps = steps; }
        
        public DefaultPipeline(params IPipelineStep[] steps)
        { Steps = steps; }
    }
}