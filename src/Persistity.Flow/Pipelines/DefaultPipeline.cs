using System.Collections.Generic;
using Persistity.Flow.Steps.Types;

namespace Persistity.Flow.Pipelines
{
    public class DefaultPipeline : FlowPipeline
    {
        public DefaultPipeline(IEnumerable<IPipelineStep> steps)
        { Steps = steps; }
        
        public DefaultPipeline(params IPipelineStep[] steps)
        { Steps = steps; }
    }
}